namespace GTBack.Core.DTO.Ecommerce.Response;

public class BasketDTO
{
    public EcommerceVariantListDTO Variants{ get; set; }
    public string? Category1{ get; set; }
    public string? Category2{ get; set; }
    public string? Category3{ get; set; }
    public int Count{ get; set; }

}