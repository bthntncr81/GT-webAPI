namespace GTBack.Core.DTO.Restourant.Request;

public class ExtraMenuItemAddOrUpdateDTO
{
    public long Id { get; set; }
    public String Name { get; set; }
    public int Price { get; set; }
    public int Stock { get; set; }
    public String Description { get; set; }
    public String Contains { get; set; }
    public String Image { get; set; }
    public long MenuItemId { get; set; }
}