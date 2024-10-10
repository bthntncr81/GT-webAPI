namespace GTBack.Core.Entities.Ecommerce;

public class EcommerceVariantOrderRelation : BaseEntity
{
    public long EcommerceVariantId { get; set; }
    public int Count { get; set; }
    public virtual EcommerceVariant EcommerceVariant { get; set; }
    public long EcommerceOrderId { get; set; }
    public virtual EcommerceOrder EcommerceOrder { get; set; }
}