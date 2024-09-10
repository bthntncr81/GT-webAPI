using GTBack.Core.DTO.Coach.Request;
using GTBack.Core.Entities.Coach;
using GTBack.Core.Enums.Coach;
using GTBack.Core.Services.coach;
using System.Linq;
using GTBack.Core.DTO.Coach.Response;
using XAct;

namespace GTBack.Service.Services.Coach;
using GTBack.Core.DTO;
using GTBack.Core.Entities;
using GTBack.Core.Results;
using GTBack.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

public class SubjectService : ISubjectService
{
    private readonly IService<Lesson> _lessonService;
    private readonly IService<SubjectScheduleRelation> _scheduleSubkectRelationService;
    private readonly IService<SubLesson> _subLessonService;
    private readonly IService<Subject> _subjectService;
    private readonly IService<Schedule> _scheduleService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClaimsPrincipal? _loggedUser;

    public SubjectService(IService<SubjectScheduleRelation> scheduleSubkectRelationService,IService<SubLesson> subLessonService, IService<Lesson> lessonService,IService<Subject> subjectService, IService<Schedule> scheduleService, IHttpContextAccessor httpContextAccessor)
    {
        _subjectService = subjectService;
        _scheduleSubkectRelationService = scheduleSubkectRelationService;
        _scheduleService = scheduleService;
        _subLessonService = subLessonService;
        _loggedUser = httpContextAccessor.HttpContext?.User;
        _httpContextAccessor = httpContextAccessor;
        _lessonService = lessonService;
        _httpContextAccessor = httpContextAccessor;
    }

    // Helper method to extract studentId from the token
    private long GetStudentIdFromToken()
    {
        var userIdClaim = _loggedUser.FindFirstValue("Id");

      
        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("StudentId not found in token.");
        }

        return long.Parse(userIdClaim);
    }

    // Add a subject for the student
    public async Task<IResults> AddSubjectToStudent(long sublessonId, DayOfWeekEnum day, string timeSlot)
    {
        long studentId = GetStudentIdFromToken(); // Get studentId from token

        // var subject = await _subjectService.GetByIdAsync(x => x.Id == subjectId);
        // if (subject == null)
        // {
        //     return new ErrorResult("Subject not found");
        // }
        var existingSchedule = await _scheduleService
            .Where(s => s.StudentId == studentId && s.SubLessonId == sublessonId && s.DayOfWeek == day && s.TimeSlot == timeSlot)
            .FirstOrDefaultAsync();



        if (existingSchedule != null)
        {
            return new ErrorResult("Subject is already scheduled for this student at the specified time");
        }

        var schedule = new Schedule
        {
            StudentId = studentId,
            SubLessonId = sublessonId,
            DayOfWeek = day,
            TimeSlot = timeSlot
        };

        await _scheduleService.AddAsync(schedule);
        return new SuccessResult("Subject added to student schedule successfully");
    }
    
    public async Task<IResults> AddSubjectOnSubLesson(long subjectId,int scheduleId,DateTime ExpireDate)
    {

        var subject = await _subjectService.GetByIdAsync(x => x.Id == subjectId);
        if (subject == null)
        {
            return new ErrorResult("Subject not found");
        }
        var existingSchedule = await _scheduleService
            .Where(s => s.Id == scheduleId )
            .FirstOrDefaultAsync();

       var isCorrect=await _subLessonService.AnyAsync(x => x.Id == subject.SubLessonId);
       if (!isCorrect)
       {
           return new ErrorResult("Subject is not connected this subLesson");

       }

       var secSub = new SubjectScheduleRelation()
       {
           SubjectId = subjectId,
           ScheduleId = scheduleId,
           ExpireDate = ExpireDate
       };
        await _scheduleSubkectRelationService.AddAsync(secSub);
        return new SuccessResult("Subject added to student schedule successfully");
    }

    
    

    // Update a subject for the student
    public async Task<IResults> UpdateSubjectForStudent(long subjectId, DayOfWeekEnum day, string timeSlot)
    {
        long studentId = GetStudentIdFromToken(); // Get studentId from token

        var schedule = await _scheduleSubkectRelationService
            .Where(s =>  s.SubjectId == subjectId)
            .FirstOrDefaultAsync();

        if (schedule == null)
        {
            return new ErrorResult("Subject not found for the student");
        }

        schedule.SubjectId = subjectId;

        await _scheduleSubkectRelationService.UpdateAsync(schedule);
        return new SuccessResult("Subject updated in student schedule successfully");
    }

    // Delete a subject for the student
    public async Task<IResults> DeleteSubjectForStudent(long scheduleId)
    {
        long studentId = GetStudentIdFromToken(); // Get studentId from token

        var schedule = await _scheduleService
            .Where(s => s.Id==scheduleId )
            .FirstOrDefaultAsync();

        if (schedule == null)
        {
            return new ErrorResult("Subject not found for the student at the specified time");
        }

        await _scheduleService.RemoveAsync(schedule);
        return new SuccessResult("Subject removed from student schedule successfully");
    }
    
    
    public async Task<IDataResults<List<LessonAddDTO>>> GetAllLessonsWithSubLessonsAndSubjects()
    {
        // Fetch lessons with their sub-lessons and subjects
        var lessons = await _lessonService.Where(x=>!x.IsDeleted)
            .Include(l => l.SubLessons)
            .ThenInclude(sl => sl.Subjects)
            .ToListAsync();

        // Convert the entity list to DTO list
        var lessonDtos = lessons.Select(lesson => new LessonAddDTO
        {
            Id = lesson.Id,
            Name = lesson.Name,
            Description = lesson.Description,
            SubLessons = lesson.SubLessons.Select(subLesson => new SubLessonAddDTO
            {
                Id = subLesson.Id,
                Name = subLesson.Name,
                Description = subLesson.Description,
                Subjects = subLesson.Subjects.Select(subject => new SubjectAddDTO()
                {
                    Id = subject.Id,
                    Name = subject.Name,
                    Description = subject.Description
                }).ToList()
            }).ToList()
        }).ToList();

        return new SuccessDataResult<List<LessonAddDTO>>(lessonDtos);
    }
    public async Task<IDataResults<Dictionary<string, List<ScheduleResponseDTO>>>> GetSubjectsByStudentIdGroupedByDay(long studentId)
    {
        // Fetch all schedules for the student
        var schedules = await _scheduleService
            .Where(s => !s.IsDeleted && s.StudentId == studentId)
            .Include(s => s.SubLesson)
            .ThenInclude(sl => sl.Subjects) // Include related Subjects in SubLesson
            .ToListAsync();
        var time = DateTime.Now.ToUniversalTime();
        var sub = await _scheduleSubkectRelationService
            .Where(x => x.ExpireDate > time && x.ScheduleId == schedules[0].Id).FirstOrDefaultAsync();
        var id = sub.SubjectId;
        // Group the subjects by day of the week
        var subjectsGroupedByDay = schedules
            .GroupBy(s => s.DayOfWeek.ToString()) // Group by DayOfWeek as a string
            .ToDictionary(
                group => group.Key, // Key: Day of the week
                group => group.Select(s => new ScheduleResponseDTO
                {
                    Id = s.Id,
                    Sublesson = s.SubLesson?.Name ?? string.Empty, // Get SubLesson Name

                    // Check if SubjectScheduleRelations is not null and has a valid entry
                    Name = s.SubjectScheduleRelations != null 
                        ? s.SubjectScheduleRelations
                            .Where(x => x.ExpireDate > time && x.Subject != null)
                            .Select(x => x.Subject.Name)
                            .FirstOrDefault() ?? string.Empty // Default to empty string if no valid Subject is found
                        : string.Empty,

                    Description = s.SubjectScheduleRelations != null
                        ? s.SubjectScheduleRelations
                            .Where(x => x.ExpireDate > time && x.Subject != null)
                            .Select(x => x.Subject.Description)
                            .FirstOrDefault() ?? string.Empty // Default to empty string if no valid Description is found
                        : string.Empty,

                    TimeSlot = s.TimeSlot
                }).ToList()
            );
        return new SuccessDataResult<Dictionary<string, List<ScheduleResponseDTO>>>(subjectsGroupedByDay);
    }
        
        public async Task<IDataResults<List<ScheduleResponseDTO>>> GetSubjectsByStudentIdAndDay(long studentId, DayOfWeekEnum day)
        {
            var schedules = await _scheduleService
                .Where(s => !s.IsDeleted && s.StudentId == studentId && s.DayOfWeek == day)
                .Include(s => s.SubLesson)
                .ThenInclude(sl => sl.Subjects) // Include related Subjects in SubLesson
                .ToListAsync();

// Map the results to ScheduleResponseDTO
            var response = schedules.Select(schedule => new ScheduleResponseDTO
            {
                Id = schedule.Id,
                Sublesson = schedule.SubLesson?.Name ?? string.Empty, // Get SubLesson Name
                Name = schedule.SubjectScheduleRelations.Where(x=>x.ExpireDate>DateTime.Now).FirstOrDefault().Subject.Name, // Get Subject Name
                Description = schedule.SubjectScheduleRelations.Where(x=>x.ExpireDate>DateTime.Now).FirstOrDefault().Subject.Name, // Get Subject Description
                TimeSlot = schedule.TimeSlot
            }).ToList();
  

        return new SuccessDataResult<List<ScheduleResponseDTO>>(response);
    }
        
        
        

}
