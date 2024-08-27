namespace GTBack.Core.DTO.Ecommerce.Request;

public class EcommerceVariantUpdateDTO
{
    public long Id { get; set; }
    public long EcommerceProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string VariantIndicator { get; set; }
    public int Stock { get; set; }
    public int Price { get; set; }
    
    public List<string> Images { get; set; }
}