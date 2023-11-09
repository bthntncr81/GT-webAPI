using GTBack.Core.Enums.Restourant;

namespace GTBack.Core.DTO.Restourant.Request;

public class ChangeOrderStatusDTO
{
    public long OrderId { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string ChangeNote { get; set; }

}