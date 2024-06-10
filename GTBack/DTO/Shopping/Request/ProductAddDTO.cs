using System.Collections.ObjectModel;
using GTBack.Core.Entities.Shopping;
using GTBack.Core.Enums.Shopping;

namespace GTBack.Core.DTO.Shopping.Request;

public class ProductAddDTO
{
    public long Id{ get; set; }
    public string Name { get; set; }
    public string MainCategory { get; set; }
    public string SubCategory { get; set; }
    public string TopCategory { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public int Stock { get; set; }
    public CategoryEnum CategoryId { get; set; }
    public CollectionEnum CollectionId { get; set; }
    public long ShoppingCompanyId { get; set; }
    public Collection<ImageAddDTO> Image { get; set; }
}