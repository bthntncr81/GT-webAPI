namespace GTBack.Core.DTO.Restourant.Response;

public class AdditionListDTO
{
    public long? Id { get; set; }
    public String Name { get; set; }
    public String TableAreaName { get; set; }
    public int TableNumber { get; set; }
    public long TableAreaId { get; set; }
    public long TableId { get; set; }
    public long? ClientId { get; set; }
    public bool IsClosed { get; set; }
    public DateTime? ClosedDate { get; set; }
    public DateTime CreatedDate { get; set; }
}