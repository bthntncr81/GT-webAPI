namespace GTBack.Core.DTO;

public class EventTypeAddRequestDTO
{
    public String Description { get; set; }
    public String Name { get; set; }
    public int Price { get; set; }
    public int Duration { get; set; }
    public long CompanyId { get; set; }
}