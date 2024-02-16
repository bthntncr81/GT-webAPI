namespace GTBack.Core.DTO.Shopping.Response;

public class GlobalProductModelResponseDTO
{
    public long?  Id { get; set; }
    public string?  ProductId { get; set; }
    public string?  ProductCode { get; set; }
    public string Barcode { get; set; }
    public string?MainCategory { get; set; }
    public string? TopCategory { get; set; }
    public string? SubCategory { get; set; }
    public string? Category { get; set; }
    public string?  BrandId { get; set; }
    public string?  Brand { get; set; }
    public string?  Name { get; set; }
    public string? Description { get; set; }
    public string?  Images { get; set; }
    public string?  NotDiscountedPrice { get; set; }
    public string?  Price { get; set; }
    public string?  Quantity { get; set; }
    public IList<MyVariant>?  Variants { get; set; }
    public string?  Detail { get; set; }
}