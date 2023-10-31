namespace GTBack.Core.DTO.Restourant.Request;

public class ExtraMenuItemAddDTO
{
    public String Name { get; set; }
    public int Price { get; set; }
    public int Stock { get; set; }
    public String Description { get; set; }
    public String Contains { get; set; }
    public String Image { get; set; }
    public long MenuItemId { get; set; }
}