using GTBack.Core.Enums.Shopping;

namespace GTBack.Core.DTO.Ecommerce.Response;

public class EcommerceOrderListDTO
{
    public long Id { get; set; }
    public long? EcommerceClientId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
    public string? Mail { get; set; }
    public string? OpenAddress { get; set; }
    public string? OrderGuid { get; set; }
    public int TotalPrice { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? District { get; set; }
    public string? IyzicoTransactionId { get; set; }
    public List<EcommerceVariantListWithCountDTO> Products { get; set; }
    public string? Note { get; set; }
    public DateTime? CreatedDate { get; set; }
    public OrderStatusEnum Status { get; set; }
}
