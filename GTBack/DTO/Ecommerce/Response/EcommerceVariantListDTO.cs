using GTBack.Core.Entities.Ecommerce;

namespace GTBack.Core.DTO.Ecommerce.Response;

public class EcommerceVariantListDTO : BaseEntity
{
    public long EcommerceProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ThumbImage { get; set; }
    public string? VariantCode { get; set; }

    public string VariantName { get; set; }
    public string VariantIndicator { get; set; }
    public int Stock { get; set; }
    public int Price { get; set; }

    public List<string> Images { get; set; }
}