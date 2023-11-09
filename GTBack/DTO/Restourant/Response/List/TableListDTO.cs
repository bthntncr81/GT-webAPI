namespace GTBack.Core.DTO.Restourant.Response;

public class TableListDTO
{    
    public long? Id { get; set; }
    public String Name { get; set; }
    public int TableNumber { get; set; }
    public long? ActiveClientId { get; set; }
    public long? ActiveAdditionId { get; set; }
    public int Capacity { get; set; }
    public long TableAreaId { get; set; }
}