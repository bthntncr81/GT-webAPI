using GTBack.Core.DTO.Coach.Request;
using GTBack.Core.Entities.Coach;
using GTBack.Core.Enums.Coach;
using GTBack.Core.Services.coach;
using System.Linq;
using AutoMapper;
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

public class ClasroomService : IClassroomService
{
    private readonly IService<Classroom> _classroomService;
    private readonly IService<SubjectScheduleRelation> _scheduleSubkectRelationService;
    private readonly IService<Student> _studentService;
    private readonly IService<Subject> _subjectService;
    private readonly IService<Schedule> _scheduleService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClaimsPrincipal? _loggedUser;
    private readonly IMapper _mapper;

    public ClasroomService(IMapper mapper, IService<SubjectScheduleRelation> scheduleSubkectRelationService, IService<Student> studentService, IService<Classroom> classroomService, IService<Subject> subjectService, IService<Schedule> scheduleService, IHttpContextAccessor httpContextAccessor)
    {
        _subjectService = subjectService;
        _mapper = mapper;
        _scheduleSubkectRelationService = scheduleSubkectRelationService;
        _scheduleService = scheduleService;
        _studentService = studentService;
        _loggedUser = httpContextAccessor.HttpContext?.User;
        _httpContextAccessor = httpContextAccessor;
        _classroomService = classroomService;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<IResults> AddClassroom(ClassroomAddDTO model)
    {
        var userIdClaim = _loggedUser.FindFirstValue("Id");

        var classroom = new Classroom()
        {
            CoachId = long.Parse(userIdClaim),
            Name = model.Name,
            Grade = model.Grade
        };
        await _classroomService.AddAsync(classroom);

        return new SuccessResult();
    }


    public async Task<IDataResults<List<ClassroomListModel>>> GetClassrooms()
    {
        var userIdClaim = _loggedUser.FindFirstValue("Id");

        var classList = await _classroomService.Where(x => x.CoachId == long.Parse(userIdClaim)).Select(x => new ClassroomListModel()
        {
            Name = x.Name,
            CoachId = x.CoachId,
            Id = x.Id
        }).ToListAsync();
        return new SuccessDataResult<List<ClassroomListModel>>(classList);
    }



    public async Task<IDataResults<List<StudentResponseDTO>>> GetClassStudents(long classId)
    {

        var student = await _studentService.Where(x => x.ClassroomId == classId).Select(s => new StudentResponseDTO()
        {
            Id = s.Id,
            Name = s.Name,
            Surname = s.Surname,
            HavePermission = s.HavePermission,
            Grade = s.Grade,
            ClassroomId = s.ClassroomId.HasValue ? (long)s.ClassroomId : 0,
            Phone = s.Phone,
            Email = s.Email,
        }).ToListAsync();
        return new SuccessDataResult<List<StudentResponseDTO>>(student);
    }

    public async Task<IResults> AddClassRoomToStudents(AddStudentToClassroom model)
    {

        foreach (var id in model.StudentIds)
        {
            var item = await _studentService.Where(x => x.Id == id).FirstOrDefaultAsync();
            item.ClassroomId = model.ClassroomId;
            await _studentService.UpdateAsync(item);
        }

        return new SuccessResult();
    }





    public async Task<IResults> AddScheduleToClass(AddScheduleAllClass model)
    {
        var studentIds = await _studentService.Where(x => x.ClassroomId == model.ClassId).Select(x => x.Id).ToListAsync();
        Guid g = Guid.NewGuid();

        foreach (var studentId in studentIds)
        {

            var schedule = new Schedule
            {
                StudentId = studentId,
                SubLessonId = model.SublessonId,
                DayOfWeek = model.Day,
                TimeSlot = model.TimeSlot,
                UniqueId = g.ToString()
            };

            await _scheduleService.AddAsync(schedule);
        }

        return new SuccessResult("Subject added to student schedule successfully");
    }



    public async Task<IResults> AddSubjectOnSubLesson(AddSubjectAllClassesDTO model)
    {

        var schedulesIds = await _scheduleService.Where(x => x.UniqueId == model.UniqueId).Select(x => x.Id).ToListAsync();

        var subject = await _subjectService.GetByIdAsync(x => x.Id == model.SubjectId);
        if (subject == null)
        {
            return new ErrorResult("Subject not found");
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
        var existingRel = await _scheduleSubkectRelationService.Where(x => x.UniqueId == model.UniqueIdSubject).ToListAsync();
        if (existingRel.IsNull() || existingRel.Count == 0)
        {
            Guid g = Guid.NewGuid();

            foreach (var schedulesId in schedulesIds)
            {

                var secSub = new SubjectScheduleRelation()
                {
                    SubjectId = model.SubjectId,
                    ScheduleId = schedulesId,
                    UniqueId = g.ToString(),
                    ExpireDate = sundayAtMidnightUtc,
                    QuestionCount = model.QuestionCount,
                    IsDone = false,
                };


                await _scheduleSubkectRelationService.AddAsync(secSub);
            }
        }
        else
        {

            foreach (var rel in existingRel)
            {
                rel.SubjectId = model.SubjectId;
                rel.QuestionCount = model.QuestionCount;
                await _scheduleSubkectRelationService.UpdateAsync(rel);
            }

        }




        return new SuccessResult("Subject added to student schedule successfully");
    }
}
