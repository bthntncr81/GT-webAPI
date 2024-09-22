using GTBack.Core.Enums.Coach;

namespace GTBack.Core.DTO.Coach.Request;

public class AddScheduleAllClass
{
    public long SublessonId { get; set; }
    public long ClassId { get; set; }
    public string? TimeSlot { get; set; }
    public DayOfWeekEnum Day { get; set; }
}

