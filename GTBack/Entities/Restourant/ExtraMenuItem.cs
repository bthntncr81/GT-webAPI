namespace GTBack.Core.Entities.Restourant;

public class ExtraMenuItem:BaseEntity
{
    public String Name { get; set; }
    public int Price { get; set; }
    public int Stock { get; set; }
    public int EstimatedTime { get; set; }
    public String Description { get; set; }
    public String Contains { get; set; }
    public String Image { get; set; }
    public long MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; }
}