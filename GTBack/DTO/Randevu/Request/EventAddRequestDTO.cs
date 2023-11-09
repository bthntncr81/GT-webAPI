namespace GTBack.Core.DTO;

public class EventAddRequestDTO
{
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public String Description { get; set; }
    public long CurrencyId { get; set; }
    public int StatusId { get; set; }
    public long AdminUserId { get; set; }
    public long ClientUserId { get; set; }
    public long EventTypeId { get; set; }
}