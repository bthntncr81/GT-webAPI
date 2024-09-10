namespace GTBack.Core.Entities.Coach;

public class Lesson:BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<SubLesson> SubLessons { get; set; } // Bu derse ait programlar

}