using GTBack.Core.Entities.Shopping;
using GTBack.Core.Enums.Shopping;

namespace GTBack.Core.DTO.Shopping.Request;

public class OrderDTO
{
    public long EcommerceClientId { get; set; }
    public string Phone { get; set; }
    public string? Mail { get; set; }
    public string? OpenAddress { get; set; }
    public string? OrderGuid { get; set; }
    public int TotalPrice { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? District { get; set; }
    public string? IyzicoTransactionId { get; set; }
    public List<int>? VariantIds { get; set; }
    public string? Note { get; set; }
    public OrderStatusEnum Status { get; set; }
}
