using GTBack.Core.Models;

namespace GTBack.Core.DTO.Ecommerce.Request;

public class EcommerceProductListFilterRepresent
{    public RequestFilter? Category1 { get; set; }
    public RequestFilter? Category2 { get; set; }
    public RequestFilter? Category3 { get; set; }
    public RequestFilter? CompanyId { get; set; }
    public RequestFilter? Name { get; set; }
    public RequestFilter? Description { get; set; }
    public RequestFilter? Price { get; set; }
    
    public EcommerceProductListFilterRepresent()
    {
        CompanyId = new RequestFilter("CompanyId","int");
        Category1 = new RequestFilter("Category1","string");
        Category2= new RequestFilter("Category2","string");
        Category3 = new RequestFilter("Category3","string");
        Name = new RequestFilter("Name","string");
        Description = new RequestFilter("Description","string");
        Price = new RequestFilter("Price","range");
        
    }
}

