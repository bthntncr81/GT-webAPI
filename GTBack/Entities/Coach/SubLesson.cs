namespace GTBack.Core.Entities.Coach;

public class SubLesson:BaseEntity
{
    public Lesson Lesson { get; set; }
    public long LessonId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<Subject> Subjects { get; set; } // Bu derse ait programlar
    
    public ICollection<Schedule> Schedules { get; set; } // Bu derse ait programlar


}