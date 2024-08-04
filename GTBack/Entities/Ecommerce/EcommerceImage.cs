namespace GTBack.Core.Entities.Ecommerce;

public class EcommerceImage:BaseEntity
{
    public string? Data { get; set; }
    public long EcommerceVariantId { get; set; }
    public EcommerceVariant EcommerceVariant { get; set; }

}