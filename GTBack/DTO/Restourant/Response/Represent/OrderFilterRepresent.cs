using GTBack.Core.Models;

namespace GTBack.Core.DTO.Restourant.Response;

public class OrderFilterRepresent
{
    public RequestFilter Name { get; set; }
    public RequestFilter OrderNote { get; set; }
    public RequestFilter? ExtraMenuItemId { get; set; }
    public RequestFilter AdditionId { get; set; }
    public RequestFilter EmployeeId { get; set; }
    public RequestFilter OrderStatus { get; set; }
    public RequestFilter OrderStartDate { get; set; }
    public RequestFilter OrderDeliveredDate { get; set; }
    
    public OrderFilterRepresent()
    {
        Name = new RequestFilter("Name","string");
        OrderNote = new RequestFilter("OrderNote","string");
        ExtraMenuItemId = new RequestFilter("ExtraMenuItemId","dropdown");
        AdditionId = new RequestFilter("AdditionId","int");
        EmployeeId = new RequestFilter("EmployeeId","dropdown");
        OrderStatus = new RequestFilter("OrderStatus","dropdown");
        OrderStartDate = new RequestFilter("OrderStartDate","date");
        OrderDeliveredDate = new RequestFilter("OrderDeliveredDate","date");
    }
}
