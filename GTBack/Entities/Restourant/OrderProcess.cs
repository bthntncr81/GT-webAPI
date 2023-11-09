using GTBack.Core.Enums.Restourant;

namespace GTBack.Core.Entities.Restourant;

public class OrderProcess:BaseEntity
{
    public String? ChangeNote { get; set; }
    public OrderStatus? InitialOrderStatus { get; set; }
    public OrderStatus? FinishedOrderStatus { get; set; }
    public DateTime ChangeDate { get; set; }
    public long OrderId { get; set; }
    public long EmployeeId { get; set; }
    public Order Order { get; set; }
    public Employee Employee { get; set; }
}