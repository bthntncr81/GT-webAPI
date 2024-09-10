using GTBack.Core.DTO.Coach.Request;
using GTBack.Core.DTO.Coach.Response;
using GTBack.Core.Enums.Coach;

namespace GTBack.Core.Services.coach;
using GTBack.Core.DTO;
using GTBack.Core.Results;
using System.Threading.Tasks;

public interface ISubjectService
{
    Task<IResults> AddSubjectToStudent(long sublessonId, DayOfWeekEnum day, string timeSlot);
    Task<IResults> UpdateSubjectForStudent(long subjectId, DayOfWeekEnum day, string timeSlot);
    Task<IResults> DeleteSubjectForStudent(long scheduleId);
    Task<IDataResults<List<LessonAddDTO>>> GetAllLessonsWithSubLessonsAndSubjects();
    
    Task<IDataResults<List<ScheduleResponseDTO>>> GetSubjectsByStudentIdAndDay(long studentId, DayOfWeekEnum day);

    Task<IDataResults<Dictionary<string, List<ScheduleResponseDTO>>>> GetSubjectsByStudentIdGroupedByDay(long studentId);

    Task<IResults> AddSubjectOnSubLesson(long subjectId, int scheduleId, DateTime ExpireDate);

}