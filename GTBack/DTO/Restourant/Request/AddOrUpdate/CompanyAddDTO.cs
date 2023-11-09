namespace GTBack.Core.DTO.Restourant.Request;

public class CompanyAddDTO:EmployeeRegisterDTO
{
    public String CompanyName { get; set; }
    public String CompanyAddress { get; set; }
    public String CompanyMail { get; set; }
    public String CompanyPhone { get; set; }
    public String Password { get; set; }
    public double? Lat { get; set; }
    public double? Lng { get; set; }
    public long? MenuId { get; set; }
}