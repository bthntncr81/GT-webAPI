namespace GTBack.Core.DTO.Restourant.Request;

public class DepartmentAddDTO
{
    public String Name { get; set; }
    public String? Mail { get; set; }
    public String? Phone { get; set; }
    public long RestoCompanyId { get; set; }
}