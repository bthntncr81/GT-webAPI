using GTBack.Core.Enums.Shopping;
using GTBack.Core.Models;

namespace GTBack.Core.DTO.Shopping.Filter;

public class ProductFilter
{
    public string? Name { get; set; }
    public string? MainCategory { get; set; }
    public string? SubCategory { get; set; }
    public string? TopCategory { get; set; }
    public string? Description { get; set; }
    public int?Sort { get; set; }
    public RangeFilter? Price { get; set; }
    public RangeFilter? Stock { get; set; }
    public string? GeneralFilter { get; set; }
    
    
    public List<CollectionEnum>?CollectionEnum { get; set; }
}