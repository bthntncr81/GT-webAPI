using GTBack.Core.Enums.Coach;

namespace GTBack.Core.Entities.Coach;

public class Classroom:BaseEntity
{
    public string Name { get; set; }
    
    public  long CoachId { get; set; }
    public virtual Coach Coach { get; set; }
    public GradeEnum Grade { get; set; }
    public ICollection<Student> Students { get; set; }
}