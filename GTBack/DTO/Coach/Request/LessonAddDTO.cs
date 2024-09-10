namespace GTBack.Core.DTO.Coach.Request;

public class LessonAddDTO
{
    
    public long Id { get; set; } = 0;
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<SubLessonAddDTO> SubLessons { get; set; }
}