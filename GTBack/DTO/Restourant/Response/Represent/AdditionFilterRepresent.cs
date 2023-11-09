using GTBack.Core.Models;

namespace GTBack.Core.DTO.Restourant.Response;

public class AdditionFilterRepresent
{
    public RequestFilter Name { get; set; }
    public RequestFilter TableAreaName { get; set; }
    public RequestFilter TableNumber { get; set; }
    public RequestFilter? ClientId { get; set; }
    public RequestFilter IsClosed { get; set; }
    public RequestFilter? ClosedDate { get; set; }
    public RequestFilter CreatedDate { get; set; }
    
    public AdditionFilterRepresent()
    {
        Name = new RequestFilter("Name","string");
        TableAreaName = new RequestFilter("TableAreaName","string");
        TableNumber = new RequestFilter("TableNumber","int");
        ClientId = new RequestFilter("ClientId","int");
        IsClosed = new RequestFilter("IsClosed","checkbox");
        ClosedDate = new RequestFilter("ClosedDate","date");
        CreatedDate = new RequestFilter("CreatedDate","date");
    }
}