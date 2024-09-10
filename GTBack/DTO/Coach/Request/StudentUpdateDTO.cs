using GTBack.Core.Enums.Coach;

namespace GTBack.Core.DTO.Coach.Request;

public class StudentUpdateDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public GradeEnum Grade { get; set; }
}