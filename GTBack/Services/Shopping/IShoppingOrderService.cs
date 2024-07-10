using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.DTO.Shopping.Response;
using GTBack.Core.Results;

namespace GTBack.Core.Services.Shopping;

public interface IShoppingOrderService
{
    Task<IDataResults<ICollection<ShoppingOrderListDTO>>> GetOrdersByUserId(int id);
    Task<IDataResults<ICollection<ShoppingOrderListDTO>>> GetOrderByOrderId(int id);
    Task<IDataResults<OrderConfirmDTO>> CreateOrder(OrderConfirmDTO model);
}