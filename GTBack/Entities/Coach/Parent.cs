namespace GTBack.Core.Entities.Coach;

public class Parent:BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string InitialPassword { get; set; }
    public long StudentId { get; set; }
    public string? PasswordHash { get; set; } // To store hashed password
    public string? ActiveForgotLink { get; set; }
    public Student Student { get; set; } // Öğrenci'nin Koçu

}