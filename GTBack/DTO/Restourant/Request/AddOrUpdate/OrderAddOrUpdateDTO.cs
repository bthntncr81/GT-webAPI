using GTBack.Core.Enums.Restourant;

namespace GTBack.Core.DTO.Restourant.Request;

public class OrderAddOrUpdateDTO
{
    public long Id { get; set; }
    public String Name { get; set; }
    public String? OrderNote { get; set; }
    public long EmployeeId { get; set; }
    public int EstimtedTime { get; set; }
    public long? ExtraMenuItemId { get; set; }
    public long AdditionId { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public DateTime OrderStartDate { get; set; }
}