namespace GTBack.Core.Entities.Shopping;

public class ShoppingOrder:BaseEntity
{
    public string Data { get; set; }
    public long BasketId { get; set; }
    public Basket Basket { get; set; }
    public long ProductId { get; set; }
    public Product Product { get; set; }
}