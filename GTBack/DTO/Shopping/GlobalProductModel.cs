namespace GTBack.Core.DTO.Shopping;

public class GlobalProductModel:BaseEntity
{
    
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
    public ICollection<MyVariant>?  Variants { get; set; }
    public string?  Detail { get; set; }
}

   public class MyVariant:BaseEntity
{
    public virtual GlobalProductModel GlobalProductModel { get; set; }
    public virtual long GlobalProductModelId { get; set; }
    public string Size { get; set; }
    public string Quantity { get; set; }
    public string VariantId { get; set; }
}
