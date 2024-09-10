namespace GTBack.Core.Entities.Ecommerce;

public class EcommerceVariant:BaseEntity
{
    public long EcommerceProductId { get; set; }
    public virtual EcommerceProduct EcommerceProduct { get; set; }
    public string Name { get; set; }
    public string? ThumbImage { get; set; }
    public string? VariantName { get; set; }
    public string? VariantIndicator { get; set; }
    public string Description { get; set; }
    public ICollection<EcommerceImage> EcommerceImages { get; set; }
    public int Stock { get; set; }
    public int Price { get; set; }
    public virtual ICollection<EcommerceBasketProductRelation> BasketProductRelations { get; set; }
}