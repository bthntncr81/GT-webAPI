using GTBack.Core.Enums;
using GTBack.Core.Models;

namespace GTBack.Core.DTO.Shopping.Filter;

public class BpmFilter
{
    public string? MainCategory { get; set; }
    public string? SubCategory { get; set; }
    public string? Id { get; set; }
    public string? GeneralFilter { get; set; }
    public string? Size { get; set; }
    public int? OrderPrice { get; set; }
    public RangeFilter? Price { get; set; }
    public int Take { get; set; }
    public int Skip { get; set; }
    public ListOrderType Type { get; set; }
}