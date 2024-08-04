using GTBack.Core.Enums.Ecommerce;

namespace GTBack.Core.Entities.Ecommerce;

public class EcommerceEmployee:BaseEntity
{

    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? Address { get; set; }
    public string PasswordHash { get; set; }
    public string? ActiveForgotLink { get; set; }
    public EcommerceUserType UserType { get; set; }
    public long EcommerceCompanyId { get; set; }
    public virtual EcommerceCompany EcommerceCompany { get; set; }
    public virtual ICollection<EcommerceProduct> EcommerceProducts { get; set; }
    public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }

}