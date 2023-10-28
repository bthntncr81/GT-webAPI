namespace GTBack.Core.Entities.Restourant;

public class Menu:BaseEntity
{
    public String Name { get; set; }
    public int CompanyId { get; set; }
    public RestoCompany RestoCompany { get; set; }
    public  virtual  ICollection<Category>? Category { get; set; }
}