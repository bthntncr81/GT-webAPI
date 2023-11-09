namespace GTBack.Core.DTO;

public class CreateCompanyDTO
{
    public String Name { get; set; }
    public String Address { get; set; }
    public long CompanyTypeId { get; set; }
    public String Mail { get; set; }
    public String Phone { get; set; }
    public float Latitude { get; set; }
    public float Longtitude { get; set; }
}