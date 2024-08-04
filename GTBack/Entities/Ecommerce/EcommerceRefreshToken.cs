using GTBack.Core.Entities.Ecommerce;

namespace GTBack.Core.Entities.Ecommerce;

public class EcommerceRefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public EcommerceEmployee? EcommerceEmployee { get; set; }
    public int? EcommerceEmployeeId { get; set; }
    public EcommerceClient? EcommerceClient { get; set; }
    public int? EcommerceClientId { get; set; }

}