using GTBack.Core.Enums.Coach;

namespace GTBack.Core.DTO.Coach.Request;

public class StudentRegisterDTO : BaseRegisterDTO
{
    public GradeEnum Grade { get; set; }
    public string CoachGuid { get; set; }
    public string Password { get; set; }
    public bool HavePermission { get; set; }
}