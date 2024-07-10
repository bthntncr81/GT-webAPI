using GTBack.Core.Entities.Shopping;
using GTBack.Core.Enums.Shopping;

namespace GTBack.Core.DTO.Shopping.Request;

public class OrderConfirmDTO
{
    public string BasketJsonDetail { get; set; }
    public long? ShoppingUserId { get; set; }
    public string Phone { get; set; }
    public string Mail { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? OrderGuid { get; set; }
    public int TotalPrice { get; set; }
    public string IyzicoTransactionId { get; set; }
    // public string OrderDate { get; set; }
    public string OrderNote{ get; set; }
    public OrderStatusEnum Status { get; set; }
    public long  AddressId { get; set; }
    public AddressAddDTO? Address { get; set; }
}