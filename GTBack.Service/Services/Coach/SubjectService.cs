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
    public async Task<IResults> AddSubjectToStudent(long sublessonId, DayOfWeekEnum day, string timeSlot,long studentId)
    {
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
    
    public async Task<IResults> AddSubjectOnSubLesson(AddSubjectToLessonDTO model)
    {

        var subject = await _subjectService.GetByIdAsync(x => x.Id == model.SubjectId);
        if (subject == null)
        {
            return new ErrorResult("Subject not found");
        }
        var existingSchedule = await _scheduleService
            .Where(s => s.Id == model.ScheduleId )
            .FirstOrDefaultAsync();

       var isCorrect=await _subLessonService.AnyAsync(x => x.Id == subject.SubLessonId);
       if (!isCorrect)
       {
           return new ErrorResult("Subject is not connected this subLesson");

       }
       
       // Get the current date and time in local time
       DateTime now = DateTime.Now;

       // Calculate the number of days until Sunday (Sunday is considered DayOfWeek.Sunday)
       int daysUntilSunday = ((int)DayOfWeek.Sunday - (int)now.DayOfWeek + 7) % 7;

       // Find the upcoming Sunday
       DateTime nextSunday = now.AddDays(daysUntilSunday);

       // Set the time to 23:59:00 for the next Sunday in local time
       DateTime sundayAtMidnight = new DateTime(nextSunday.Year, nextSunday.Month, nextSunday.Day, 23, 59, 0, DateTimeKind.Local);

       // Convert the time to UTC before saving to the database
       DateTime sundayAtMidnightUtc = sundayAtMidnight.ToUniversalTime();
       var existingRel = await _scheduleSubkectRelationService.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
       if (existingRel.IsNull())
       {
           var secSub = new SubjectScheduleRelation()
           {
               SubjectId = model.SubjectId,
               ScheduleId = model.ScheduleId,
               ExpireDate = sundayAtMidnightUtc,
               QuestionCount = model.QuestionCount,
               IsDone = false,
           };
           await _scheduleSubkectRelationService.AddAsync(secSub);
       }
       else
       {
           existingRel.SubjectId = model.SubjectId;
           existingRel.QuestionCount = model.QuestionCount;
           await _scheduleSubkectRelationService.UpdateAsync(existingRel);

       }

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
   public async Task<IDataResults<Dictionary<string, List<ScheduleResponseDTO>>>> GetSubjectsByStudentIdGroupedByDay()
{
    long studentId = GetStudentIdFromToken(); // Get studentId from token

    // Fetch all schedules for the student with necessary includes
    var schedules = await _scheduleService
        .Where(s => !s.IsDeleted && s.StudentId == studentId)
        .Include(s => s.SubLesson)
        .ThenInclude(sl => sl.Subjects) // Include related Subjects in SubLesson
        .Include(s => s.SubjectScheduleRelations) // Ensure SubjectScheduleRelations is included
        .ThenInclude(ssr => ssr.Subject) // Include related Subject
        .AsNoTracking() // Improve performance since we are just reading data
        .ToListAsync(); // Get the data from the database

    // Now perform client-side operations
    var subjectsGroupedByDay = schedules
        .AsEnumerable() // Switch to client-side evaluation
        .GroupBy(s => s.DayOfWeek.ToString()) // Group by DayOfWeek in memory
        .ToDictionary(
            group => group.Key,
            group => group.Select(s => new ScheduleResponseDTO
            {
                Id = s.Id,
                Sublesson = s.SubLesson?.Name ?? string.Empty, // Ensure SubLesson is not null
                SublessonId = s.SubLesson?.Id ?? 0,
                ScheduleRelId = s.SubjectScheduleRelations?.FirstOrDefault(x => x.ExpireDate > DateTime.Now.ToUniversalTime() && x.Subject != null)?.Id ?? 0,
                Name = s.SubjectScheduleRelations?.FirstOrDefault(x => x.ExpireDate > DateTime.Now.ToUniversalTime() && x.Subject != null)?.Subject?.Name ?? string.Empty,
                Description = s.SubjectScheduleRelations?.FirstOrDefault(x => x.ExpireDate > DateTime.Now.ToUniversalTime() && x.Subject != null)?.Subject?.Description ?? string.Empty,
                TimeSlot = s.TimeSlot,
                QuestionCount = s.SubjectScheduleRelations?.FirstOrDefault(x => x.ExpireDate > DateTime.Now.ToUniversalTime())?.QuestionCount ?? 0,
                IsDone = s.SubjectScheduleRelations?.FirstOrDefault(x => x.ExpireDate > DateTime.Now.ToUniversalTime())?.IsDone ?? false,
                SubjectId = s.SubjectScheduleRelations?.FirstOrDefault(x => x.ExpireDate > DateTime.Now.ToUniversalTime())?.SubjectId ?? 0,
            }).ToList()
        );

    // Return the result
    return new SuccessDataResult<Dictionary<string, List<ScheduleResponseDTO>>>(subjectsGroupedByDay);
}
   
    public async Task<IDataResults<Dictionary<string, List<ScheduleResponseDTO>>>> GetSubjectsByStudentIdGroupedByDay(int studentId)
{

    // Fetch all schedules for the student with necessary includes
    var schedules = await _scheduleService
        .Where(s => !s.IsDeleted && s.StudentId == studentId)
        .Include(s => s.SubLesson)
        .ThenInclude(sl => sl.Subjects) // Include related Subjects in SubLesson
        .Include(s => s.SubjectScheduleRelations) // Ensure SubjectScheduleRelations is included
        .ThenInclude(ssr => ssr.Subject) // Include related Subject
        .AsNoTracking() // Improve performance since we are just reading data
        .ToListAsync(); // Get the data from the database

    // Now perform client-side operations
    var subjectsGroupedByDay = schedules
        .AsEnumerable() // Switch to client-side evaluation
        .GroupBy(s => s.DayOfWeek.ToString()) // Group by DayOfWeek in memory
        .ToDictionary(
            group => group.Key,
            group => group.Select(s => new ScheduleResponseDTO
            {
                Id = s.Id,
                Sublesson = s.SubLesson?.Name ?? string.Empty, // Get SubLesson Name
                SublessonId = s.SubLesson?.Id ?? 0, // Get SubLesson Name
                ScheduleRelId = s.SubjectScheduleRelations != null
                    ? s.SubjectScheduleRelations
                        .Where(x => x.ExpireDate > DateTime.Now.ToUniversalTime() && x.Subject != null)
                        .Select(x => x.Id)
                        .FirstOrDefault() 
                    : 0,
                Name = s.SubjectScheduleRelations != null
                    ? s.SubjectScheduleRelations
                        .Where(x => x.ExpireDate > DateTime.Now.ToUniversalTime() && x.Subject != null)
                        .Select(x => x.Subject.Name)
                        .FirstOrDefault() ?? string.Empty
                    : string.Empty,
                Description = s.SubjectScheduleRelations != null
                    ? s.SubjectScheduleRelations
                        .Where(x => x.ExpireDate > DateTime.Now.ToUniversalTime() && x.Subject != null)
                        .Select(x => x.Subject.Description)
                        .FirstOrDefault() ?? string.Empty
                    : string.Empty,
                TimeSlot = s.TimeSlot,
                QuestionCount = s.SubjectScheduleRelations != null 
                    ? s.SubjectScheduleRelations
                        .Where(x => x.ExpireDate > DateTime.Now.ToUniversalTime())
                        .Select(x => x.QuestionCount)
                        .FirstOrDefault() ?? 0
                    : 0,
                SubjectId = s.SubjectScheduleRelations != null 
                    ? s.SubjectScheduleRelations
                        .Where(x => x.ExpireDate > DateTime.Now.ToUniversalTime())
                        .Select(x => x.SubjectId)
                        .FirstOrDefault() 
                    : 0,
                IsDone = s.SubjectScheduleRelations != null
                    ? s.SubjectScheduleRelations
                        .Where(x => x.ExpireDate > DateTime.Now.ToUniversalTime())
                        .Select(x => x.IsDone)
                        .FirstOrDefault() 
                    : false
            }).ToList()
        );

    // Return the result
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
            var response = schedules.Select(s => new ScheduleResponseDTO
            {
                Id = s.Id,
                Sublesson = s.SubLesson?.Name ?? string.Empty, // Get SubLesson Name
                SublessonId = s.SubLesson.Id, // Get SubLesson Name
                ScheduleRelId = s.SubjectScheduleRelations != null
                    ? s.SubjectScheduleRelations
                        .Where(x => x.ExpireDate > DateTime.Now.ToUniversalTime() && x.Subject != null)
                        .Select(x => x.Id)
                        .FirstOrDefault() 
                    : 0,
                Name = s.SubjectScheduleRelations != null
                    ? s.SubjectScheduleRelations
                        .Where(x => x.ExpireDate > DateTime.Now.ToUniversalTime() && x.Subject != null)
                        .Select(x => x.Subject.Name)
                        .FirstOrDefault() ?? string.Empty
                    : string.Empty,
                Description = s.SubjectScheduleRelations != null
                    ? s.SubjectScheduleRelations
                        .Where(x => x.ExpireDate > DateTime.Now.ToUniversalTime() && x.Subject != null)
                        .Select(x => x.Subject.Description)
                        .FirstOrDefault() ?? string.Empty
                    : string.Empty,
                TimeSlot = s.TimeSlot,
                QuestionCount = s.SubjectScheduleRelations != null 
                    ? s.SubjectScheduleRelations
                        .Where(x => x.ExpireDate > DateTime.Now.ToUniversalTime())
                        .Select(x => x.QuestionCount)
                        .FirstOrDefault() 
                    : 0,
                SubjectId = s.SubjectScheduleRelations != null 
                    ? s.SubjectScheduleRelations
                        .Where(x => x.ExpireDate > DateTime.Now.ToUniversalTime())
                        .Select(x => x.SubjectId)
                        .FirstOrDefault() 
                    : 0,
                IsDone = s.SubjectScheduleRelations != null
                    ? s.SubjectScheduleRelations
                        .Where(x => x.ExpireDate > DateTime.Now.ToUniversalTime())
                        .Select(x => x.IsDone)
                        .FirstOrDefault() 
                    : false
            }).ToList();
  

        return new SuccessDataResult<List<ScheduleResponseDTO>>(response);
    }


       public  async Task<IResults> ChangeIsDone(long scheduleRelId, bool isDone)
       {

          var scheduleRel= await _scheduleSubkectRelationService.Where(x => x.Id == scheduleRelId).FirstOrDefaultAsync();
          scheduleRel.IsDone = isDone;

       await   _scheduleSubkectRelationService.UpdateAsync(scheduleRel);
            return new SuccessResult("Succesfuly Changed");

        }
        
        
        

}
