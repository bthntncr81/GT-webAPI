namespace GTBack.Core.DTO;

public class UserRegisterDTO:BaseRegisterDTO
{

    public long UserTypeId { get; set; }
    public long CompanyId { get; set; }
    public string Password { get; set; }
}