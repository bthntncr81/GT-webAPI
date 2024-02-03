using GTBack.Core.Models;

namespace GTBack.Core.DTO.Shopping.Filter;

public class GlobalShoppingFilter
{
    public string? MainCategory { get; set; }
    public string? SubCategory { get; set; }
    public string? Id { get; set; }
    public Boolean? OrderPrice { get; set; }
    public RangeFilter? Price { get; set; }

}