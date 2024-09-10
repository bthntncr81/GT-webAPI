namespace GTBack.Core.Entities.Coach;

public class Subject:BaseEntity
{
    public SubLesson SubLesson { get; set; }
    public long SubLessonId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ICollection<Schedule> Schedules { get; set; } // Bu derse ait programlar
}