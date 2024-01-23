namespace GTBack.Core.Entities.Shopping;

public class ShoppingCompany:BaseEntity
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Mail { get; set; }
    public string Phone { get; set; }
    public string Logo { get; set; }
    public virtual ICollection<ShoppingUser> ShoppingUser { get; set; }
    public virtual ICollection<Product> Product { get; set; }

}