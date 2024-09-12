namespace GTBack.Core.DTO.Coach.Response;

public class ScheduleResponseDTO
{
    public long Id { get; set; } = 0;
    public string Sublesson { get; set; }
    public long? ScheduleRelId { get; set; }
    public long? SubjectId { get; set; }
    public long SublessonId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ?QuestionCount { get; set; }
    public bool? IsDone { get; set; }
    public string TimeSlot { get; set; }
}