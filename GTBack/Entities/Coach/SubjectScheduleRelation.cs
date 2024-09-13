namespace GTBack.Core.Entities.Coach;

public class SubjectScheduleRelation:BaseEntity
{
    public long? ScheduleId { get; set; } // Programdaki ders
    public long? SubjectId { get; set; } // Programdaki ders
    
    public int? QuestionCount { get; set; } // Programdaki ders
    public int? CorrectCount { get; set; } // Programdaki ders
    public bool? IsDone { get; set; } // Programdaki ders
    public DateTime ExpireDate { get; set; } // Programdaki ders
    public Subject? Subject { get; set; } // İlişkili ders
    public Schedule? Schedule { get; set; } // İlişkili ders
    public ICollection<QuestionImage> QuestionImage { get; set; } // Bu derse ait programlar


}