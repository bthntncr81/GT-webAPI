namespace GTBack.Core.DTO;

public class EventListClientResponseDto
{
    public String Mail { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public String Description { get; set; }
    public long AdminUserId { get; set; }
    
    public UserDTO Admin { get; set; }
    public UserDTO Client { get; set; }
    public long ClientUserId { get; set; }
    public EventTypeDTO EventTypeDto { get; set; }
    public int StatusId { get; set; }
    public int Price { get; set; }
    public long Id { get; set; }
}