namespace GTBack.Core.DTO.Shopping.Request;

public class AddressAddDTO
{
    
    public string Name { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string OpenAddress { get; set; }
    public long? ShoppingOrderId { get; set; }
    public long? ShoppingUserId { get; set; }
}