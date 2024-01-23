using GTBack.Core.Enums.Shopping;

namespace GTBack.Core.Entities.Shopping;

public class Product:BaseEntity
{
    public string Name { get; set; }
    public virtual ICollection<Image> Image { get; set; }
    public int Price { get; set; }
    public int Stock { get; set; }
    public CategoryEnum CategoryId { get; set; }
    public ShoppingCompany ShoppingCompany { get; set; }
    public long ShoppingCompanyId { get; set; }


    public CollectionEnum CollectionId { get; set; }
}