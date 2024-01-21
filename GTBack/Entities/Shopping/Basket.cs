namespace GTBack.Core.Entities.Shopping;

public class Basket:BaseEntity
{
    public virtual ICollection<ShoppingOrder> ShoppingOrder { get; set; }
    public virtual ShoppingUser ShoppingUser { get; set; }
    public int ShoppingUserId { get; set; }
}