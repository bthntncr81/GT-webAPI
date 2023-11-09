namespace GTBack.Core.DTO.Restourant.Request;

public class MenuItemAddOrUpdateDTO
{
    public long Id { get; set; }
    public String Name { get; set; }
    public int Price { get; set; }
    public int Stock { get; set; }
    public String Image { get; set; }
    public String Description { get; set; }
    public String Contains { get; set; }
    public long CategoryId { get; set; }
}