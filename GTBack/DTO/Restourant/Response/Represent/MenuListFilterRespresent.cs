using GTBack.Core.Models;

namespace GTBack.Core.DTO.Restourant.Request;

public class MenuListFilterRespresent
{
    
    public RequestFilter Name { get; set; }
    public RequestFilter CategoryId { get; set; }
    public RequestFilter Price { get; set; }
    public RequestFilter Stock { get; set; }
    public RequestFilter Description { get; set; }
    public RequestFilter Contains { get; set; }
    
    public MenuListFilterRespresent()
    {
        Name = new RequestFilter("Name","string");
        CategoryId = new RequestFilter("CategoryId","int");
        Price = new RequestFilter("Price","range");
        Stock = new RequestFilter("Stock","range");
        Description = new RequestFilter("Description","string");
        Contains = new RequestFilter("Contains","string");
       
    
    }
}