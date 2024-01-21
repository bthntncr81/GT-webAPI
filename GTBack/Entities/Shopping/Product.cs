using GTBack.Core.Enums.ShoppingCart;

namespace GTBack.Core.Entities.Shopping;

public class Product:BaseEntity
{
    public string Name { get; set; }
    public virtual ICollection<Image> Image { get; set; }
    public int Price { get; set; }
    public int Stock { get; set; }
    public ShoppingCategory CategoryId { get; set; }
    public ShoppingCollection CollectionId { get; set; }
}