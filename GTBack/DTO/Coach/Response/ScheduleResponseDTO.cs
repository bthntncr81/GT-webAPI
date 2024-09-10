namespace GTBack.Core.DTO.Coach.Response;

public class ScheduleResponseDTO
{
    public long Id { get; set; } = 0;
    public string Sublesson { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string TimeSlot { get; set; }
}