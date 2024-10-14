using GTBack.Core.DTO.Coach.Request;
using GTBack.Core.DTO.Coach.Response;
using GTBack.Core.Enums.Coach;
using GTBack.Core.Results;

namespace GTBack.Core.Services.coach;

public interface IClassroomService
{
    Task<IResults> AddClassroom(ClassroomAddDTO model);

    Task<IResults> AddClassRoomToStudents(AddStudentToClassroom model);

    Task<IResults> AddScheduleToClass(AddScheduleAllClass model);
    Task<IDataResults<List<ClassroomListModel>>> GetClassrooms();
    Task<IDataResults<List<StudentUpdateDTO>>> GetClassStudents(long classId);
    Task<IResults> AddSubjectOnSubLesson(AddSubjectAllClassesDTO model);
}