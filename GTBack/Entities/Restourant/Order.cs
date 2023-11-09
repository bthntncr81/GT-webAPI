using GTBack.Core.Enums.Restourant;

namespace GTBack.Core.Entities.Restourant;

public class Order:BaseEntity
{
    
    public Order()
    {
        OrderStartDate = DateTime.UtcNow;

    }
    public String Name { get; set; }
    public String? OrderNote { get; set; }
    public long? ExtraMenuItemId { get; set; }
    public long AdditionId { get; set; }
    public long EmployeeId { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public DateTime OrderStartDate { get; set; } 
    public DateTime? OrderDeliveredDate { get; set; }
    public ExtraMenuItem? ExtraMenuItem { get; set; }
    public Employee? Employee { get; set; }
    public Addition Addition { get; set; }
    public virtual ICollection<OrderProcess> OrderProcess { get; set; }
} 