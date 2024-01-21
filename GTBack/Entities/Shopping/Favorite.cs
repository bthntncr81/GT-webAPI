namespace GTBack.Core.Entities.Shopping;

public class Favorite:BaseEntity
{
    public virtual ShoppingUser ShoppingUser { get; set; }
    public virtual Product Product { get; set; }
    public int ShoppingUserId { get; set; }
    public int ProductId { get; set; }
}