namespace GTBack.Core.Entities.Ecommerce;

public class EcommerceProductOrderRelation
{
    public long EcommerceProductId { get; set; }
    public virtual EcommerceProduct EcommerceProduct { get; set; }
    public long EcommerceOrderId { get; set; }
    public virtual EcommerceOrder EcommerceOrder { get; set; }
}