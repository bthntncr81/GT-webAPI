namespace GTBack.Core.DTO;

public class GoogleLoginDTO
{
    public string Email { get; set; }
    public string Id { get; set; }
    public string IdToken { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Provider { get; set; }
}