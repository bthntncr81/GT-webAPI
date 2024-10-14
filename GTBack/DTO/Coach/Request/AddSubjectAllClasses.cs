using GTBack.Core.Enums.Coach;

namespace GTBack.Core.DTO.Coach.Request;

public class AddSubjectAllClassesDTO
{

    public long SubjectId { get; set; }
    public string UniqueId { get; set; }
    public string UniqueIdSubject { get; set; }
    public int QuestionCount { get; set; }
}