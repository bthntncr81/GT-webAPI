using GTBack.Core.DTO.Restourant.Request;

namespace GTBack.Core.DTO.Restourant.Response.List;

public class MenuAndExtrasListDTO
{
    public MenuItemListDTO MenuItem { get; set; }
    public List<ExtraMenuItemListDTO> ExtraMenuItemList { get; set; }
}