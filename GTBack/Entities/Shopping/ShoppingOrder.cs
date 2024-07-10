using GTBack.Core.Enums.Shopping;

namespace GTBack.Core.Entities.Shopping;

public class ShoppingOrder:BaseEntity
{
    public string BasketJsonDetail { get; set; }
    public string OrderGuid { get; set; }
    public long? ShoppingUserId { get; set; }
    public int TotalPrice { get; set; }
    
    public string Name { get; set; }
    public string Surname { get; set; }
    public string IyzicoTransactionId { get; set; }
    
    public string Phone { get; set; }
    public string OrderNote{ get; set; }

    public string Mail { get; set; }
    // public DateTime OrderDate { get; set; }
    public OrderStatusEnum Status { get; set; }
    public Address  Address { get; set; }
    public virtual ShoppingUser? ShoppingUser { get; set; }

    public long  AddressId { get; set; }
 
}