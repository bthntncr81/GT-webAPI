using GTBack.Core.Models;

namespace GTBack.Core.DTO.Ecommerce.Request;

public class EcommerceProductFilter
{
    public long Id { get; set; }
    public string? Category1 { get; set; }
    public string? Category2 { get; set; }
    public string? Category3 { get; set; }
    public int? CompanyId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public RangeFilter? Price { get; set; }
    public RangeFilter? Stock { get; set; }
    
    public string?Sort { get; set; }


}