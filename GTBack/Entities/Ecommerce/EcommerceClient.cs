namespace GTBack.Core.Entities.Ecommerce;

public class EcommerceClient:BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? Address { get; set; }
    public string? PasswordHash { get; set; }
    public string? ActiveForgotLink { get; set; }
    public long EcommerceCompanyId { get; set; }
    public virtual EcommerceCompany EcommerceCompany { get; set; }
    public long BasketId { get; set; }
    public virtual ICollection<EcommerceClientFavoriteRelation> EcommerceClientFavoriteRelations { get; set; }
    public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }

}