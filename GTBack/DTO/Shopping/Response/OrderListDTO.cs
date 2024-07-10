using GTBack.Core.Entities.Shopping;
using GTBack.Core.Enums.Shopping;

namespace GTBack.Core.DTO.Shopping.Response;

public class ShoppingOrderListDTO
{
    public string BasketJsonDetail { get; set; }
    public long? ShoppingUserId { get; set; }
    public string CardNumber { get; set; }
    public string? OrderGuid { get; set; }
    public int TotalPrice { get; set; }
    public string IyzicoTransactionId { get; set; }
    // public DateTime OrderDate { get; set; }
    public string OrderNote{ get; set; }
    
    public string Name { get; set; }
    public string Surname { get; set; }

    public OrderStatusEnum Status { get; set; }
    public AddressResponseDTO Address { get; set; }
    
    public string Phone { get; set; }
    public string Mail { get; set; }
}