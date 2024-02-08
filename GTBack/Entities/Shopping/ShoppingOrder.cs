using GTBack.Core.Enums.Shopping;

namespace GTBack.Core.Entities.Shopping;

public class ShoppingOrder:BaseEntity
{
    public string BasketJsonDetail { get; set; }
    public long? ShoppingUserId { get; set; }
    public virtual ShoppingUser? ShoppingUser { get; set; }
    public string CardNumber { get; set; }
    public string TotalPrice { get; set; }
    public string IyzicoTransactionId { get; set; }
    public string OrderDate { get; set; }
    public OrderStatusEnum Status { get; set; }
    public Address  Address { get; set; }
    public long  AddressId { get; set; }
 
}