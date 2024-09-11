using GTBack.Core.Enums.Coach;

namespace GTBack.Core.DTO.Coach.Request;

public class AddLessonToStudentDTO
{

    public string TimeSlot { get; set; }
    public long SubLessonId { get; set; }
    public long StudentId { get; set; }
    public DayOfWeekEnum day { get; set; }
}