using GTBack.Core.Models;

namespace GTBack.Core.DTO.Restourant.Request;

public class MenuListFilterDTO
{
    
    public String? Name { get; set; }
    public RangeFilter? Price { get; set; }
    public RangeFilter? Stock { get; set; }
    public String? Description { get; set; }
    public String? Contains { get; set; }
}