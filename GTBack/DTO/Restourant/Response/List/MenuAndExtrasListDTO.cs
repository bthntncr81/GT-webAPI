using GTBack.Core.DTO.Restourant.Request;

namespace GTBack.Core.DTO.Restourant.Response.List;

public class MenuAndExtrasListDTO
{
   public MenuAndExtrasListDTO()
    {
        ExtraMenuItemList = new List<ExtraMenuItemListDTO>();
    }
    public MenuItemListDTO MenuItem { get; set; }
    public List<ExtraMenuItemListDTO> ExtraMenuItemList { get; set; }
}