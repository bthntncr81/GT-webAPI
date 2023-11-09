namespace GTBack.Core.DTO.Restourant.Request;

public class TableAreaAddOrUpdateDTO
{
    public long Id { get; set; }
    public String Name { get; set; }
    public long RestoCompanyId { get; set; }
}