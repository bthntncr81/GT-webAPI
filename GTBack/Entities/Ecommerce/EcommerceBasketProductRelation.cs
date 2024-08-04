namespace GTBack.Core.Entities.Ecommerce;

public class EcommerceBasketProductRelation:BaseEntity
{
    public long EcommerceBasketId { get; set; }
    public virtual EcommerceBasket Basket { get; set; }
    public long EcommerceVariantId { get; set; }
    public int Count { get; set; }
    public virtual EcommerceVariant EcommerceVariant { get; set; }
    
}