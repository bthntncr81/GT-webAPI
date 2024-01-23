using GTBack.Core.Enums.Shopping;
using GTBack.Core.Models;

namespace GTBack.Core.DTO.Shopping.Filter;

public class ProductFilter
{
    public string? Name { get; set; }
    public RangeFilter? Price { get; set; }
    public RangeFilter? Stock { get; set; }
    
    public List<CategoryEnum>? CategoryEnum { get; set; }
    
    public List<CollectionEnum>?CollectionEnum { get; set; }
}