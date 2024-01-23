using System.Collections.ObjectModel;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Entities.Shopping;
using GTBack.Core.Enums.Shopping;

namespace GTBack.Core.DTO.Shopping.Response;

public class ProductListDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int Stock { get; set; }
    public CategoryEnum CategoryId { get; set; }
    public CollectionEnum CollectionId { get; set; }
    public List<ImageAddDTO> Image { get; set; }
}