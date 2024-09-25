using GTBack.Core.Enums.Coach;

namespace GTBack.Core.Entities.Coach;

public class Student:BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public GradeEnum Grade { get; set; }
    public long CoachId { get; set; }
    public long ParentId { get; set; }
    public long? ClassroomId { get; set; }
    public string? PasswordHash { get; set; } // To store hashed password
    public string? ActiveForgotLink { get; set; }


    public virtual ICollection<RefreshToken>? RefreshTokens { get; set; } 
    public Coach Coach { get; set; } // Öğrenci'nin Koçu
    public Parent Parent { get; set; } // Öğrenci'nin Koçu
    public Classroom? Classroom { get; set; } // Öğrenci'nin Koçu
    public ICollection<Schedule> Schedules { get; set; } // Öğrenci'nin haftalık programı
}
