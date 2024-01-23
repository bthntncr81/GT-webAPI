using GTBack.Core.Enums;

namespace GTBack.Core.Entities.Shopping;

public class ShoppingUser:BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Mail { get; set; }
    public string PasswordHash { get; set; }
    public long? ActiveBasketId { get; set; }
    public long ShoppingCompanyId { get; set; }
    public UserTypeShopping? UserTypeId { get; set; }
    public virtual ShoppingCompany ShoppingCompany { get; set; }
    public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }
}