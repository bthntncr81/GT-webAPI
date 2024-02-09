namespace GTBack.Core.Entities.Shopping;

public class Address:BaseEntity
{
    public string Name { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string OpenAddress { get; set; }
    public long? ShoppingUserId { get; set; }

    public long? ShoppingOrderId { get; set; }
    public ShoppingUser? ShoppingUser { get; set; }
    public ShoppingOrder? ShoppingOrder { get; set; }

}