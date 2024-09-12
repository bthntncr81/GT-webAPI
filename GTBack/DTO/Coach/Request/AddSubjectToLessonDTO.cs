using GTBack.Core.Enums.Coach;

namespace GTBack.Core.DTO.Coach.Request;

public class AddSubjectToLessonDTO
{
    public long Id { get; set; }
    public long SubjectId { get; set; }
    public long ScheduleId { get; set; }
    public int QuestionCount { get; set; }
}