namespace GTBack.Core.Entities.Shopping;

public class Address:BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string OpenAddress { get; set; }
    public string Phone { get; set; }
    public string Mail { get; set; }
    public string Note { get; set; }
    public ShoppingOrder? ShoppingOrder { get; set; }
    public long? ShoppingOrderId { get; set; }
    public ShoppingUser? ShoppingUser { get; set; }
    public long? ShoppingUserId { get; set; }
}