using GTBack.Core.Enums.Coach;

namespace GTBack.Core.DTO.Coach.Request;

public class ClassroomAddDTO
{
    public string Name { get; set; }
    public GradeEnum Grade { get; set; }
}