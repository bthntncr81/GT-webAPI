namespace GTBack.Core.Entities.Ecommerce;

public class EcommerceBasket:BaseEntity
{
    public virtual ICollection<EcommerceBasketProductRelation> EcommerceBasketProductRelations { get; set; }
    public string Guid { get; set; }

}