namespace GTBack.Core.Entities.Ecommerce;

public class EcommerceClientFavoriteRelation:BaseEntity
{
    public long EcommerceClientId { get; set; }
    public virtual EcommerceClient Client { get; set; }
    public long EcommerceProductId { get; set; }
    public virtual EcommerceProduct Product { get; set; }
}