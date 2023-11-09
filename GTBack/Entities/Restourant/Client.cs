namespace GTBack.Core.Entities.Restourant;

public class Client:BaseEntity
{
    public String Name { get; set; }
    public String Surname { get; set; }
    public String? Address { get; set; }
    public String Mail { get; set; }
    public String Phone { get; set; }
    public string PasswordHash { get; set; }
    public  virtual  ICollection<Reservation>? Reservation { get; set; }
    public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }

}