using GTBack.Core.Enums.Shopping;

namespace GTBack.Core.Entities.Shopping;

public class Product:BaseEntity
{
    public string Name { get; set; }
    public virtual ICollection<Image> Image { get; set; }
    public int Price { get; set; }
    public int Stock { get; set; }
    public string Description { get; set; }
    public string MainCategory { get; set; }
    public string SubCategory { get; set; }
    public string TopCategory { get; set; }
    public ShoppingCompany ShoppingCompany { get; set; }
    public long ShoppingCompanyId { get; set; }


}