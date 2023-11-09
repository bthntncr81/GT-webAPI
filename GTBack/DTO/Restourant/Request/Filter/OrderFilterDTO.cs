using GTBack.Core.Enums.Restourant;
using GTBack.Core.Models;

namespace GTBack.Core.DTO.Restourant.Request;

public class OrderFilterDTO
{
    public String? Name { get; set; }
    public String? OrderNote { get; set; }
    public long? ExtraMenuItemId { get; set; }
    public long? AdditionId { get; set; }
    public long? EmployeeId { get; set; }
    public OrderStatus? OrderStatus { get; set; }
    public DateRange? OrderStartDate { get; set; }
    public DateRange? OrderDeliveredDate { get; set; }
}