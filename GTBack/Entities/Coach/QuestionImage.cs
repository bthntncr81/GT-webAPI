namespace GTBack.Core.Entities.Coach;

public class QuestionImage:BaseEntity
{
    public string Data { get; set; }
    public long SubjectScheduleRelationId { get; set; }
    public SubjectScheduleRelation SubjectScheduleRelation { get; set; }
}