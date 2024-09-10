namespace GTBack.Core.Entities.Coach;

public class Coach:BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? PasswordHash { get; set; } // To store hashed password
    public string? ActiveForgotLink { get; set; }


    public virtual ICollection<RefreshToken>? RefreshTokens { get; set; } 
    public ICollection<Student> Students { get; set; } // Koçun öğrencileri
}