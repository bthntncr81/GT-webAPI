using GTBack.Core.Models;

namespace GTBack.Core.DTO.Shopping.Request;

public class ProductListFilterRepresent
{
    public RequestFilter Name { get; set; }
    public RequestFilter Price { get; set; }
    public RequestFilter Stock { get; set; }
    
    public RequestFilter CategoryId { get; set; }
    
    public RequestFilter CollectionId { get; set; }
    
    public ProductListFilterRepresent()
    {
        Name = new RequestFilter("Name","string");
        CategoryId = new RequestFilter("CategoryId","list");
        CollectionId = new RequestFilter("CategoryId","list");
        Price = new RequestFilter("Price","range");
        Stock = new RequestFilter("Stock","range");
        
    }
}

