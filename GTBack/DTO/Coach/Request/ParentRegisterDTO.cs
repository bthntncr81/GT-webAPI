namespace GTBack.Core.DTO.Coach.Request;

public class ParentRegisterDTO:BaseRegisterDTO
{
    public string StudentMail { get; set; }
    public string Password { get; set; }
    public string InitialPassword { get; set; }
}