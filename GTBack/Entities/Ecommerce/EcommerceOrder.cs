using GTBack.Core.Enums.Shopping;

namespace GTBack.Core.Entities.Ecommerce;

public class EcommerceOrder : BaseEntity
{
    public string OrderGuid { get; set; }
    public long? EcommerceClientId { get; set; }
    public virtual EcommerceClient EcommerceClient { get; set; }
    public int TotalPrice { get; set; }
    public string Note { get; set; }
    public OrderStatusEnum Status { get; set; }
    public string OpenAddress { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string IyzicoTransactionId { get; set; }
    public virtual ICollection<EcommerceVariantOrderRelation> EcommerceVariantOrderRelation { get; set; }
}

