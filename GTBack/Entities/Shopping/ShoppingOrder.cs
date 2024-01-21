namespace GTBack.Core.Entities.Shopping;

public class ShoppingOrder:BaseEntity
{
    public string Name { get; set; }
    public long BasketId { get; set; }
    public virtual Basket Basket { get; set; }
    public virtual Product Product { get; set; }
    public long ProductId { get; set; }

}