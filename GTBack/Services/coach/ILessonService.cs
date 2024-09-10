using GTBack.Core.DTO.Coach.Request;
using GTBack.Core.Entities.Coach;
using GTBack.Core.Results;

namespace GTBack.Core.Services.coach;

public interface ILessonService
{
    Task<IResults> AddLessonWithSubLessonsAndSubjects(LessonAddDTO lessonDto);

}