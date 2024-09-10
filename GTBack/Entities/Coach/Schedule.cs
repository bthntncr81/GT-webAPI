using GTBack.Core.Enums.Coach;

namespace GTBack.Core.Entities.Coach;

public class Schedule:BaseEntity
{
    public long StudentId { get; set; } // Programın ait olduğu öğrenci
    public DayOfWeekEnum DayOfWeek { get; set; } // Haftanın günü
    // public long? SubjectId { get; set; } // Programdaki ders
    public long SubLessonId { get; set; } // Programdaki ders
    public string TimeSlot { get; set; } // Dersin saat dilimi

    public Student Student { get; set; } // İlişkili öğrenci
    // public Subject? Subject { get; set; } // İlişkili ders
    public SubLesson SubLesson { get; set; } // İlişkili ders
    
    public ICollection<SubjectScheduleRelation> SubjectScheduleRelations { get; set; } // Bu derse ait programlar

}