
namespace GTBack.Core.Entities.Restourant;

public class Category:BaseEntity
{
    public String Name { get; set; }
    public long MenuId { get; set; }
    public Menu Menu { get; set; }
    public string Image { get; set; }
    public  virtual  ICollection<MenuItem> MenuItem { get; set; }
}