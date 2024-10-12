using GTBack.Core.DTO.Ecommerce.Response;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.DTO.Shopping.Response;
using GTBack.Core.Results;

namespace GTBack.Core.Services.Shopping;

public interface IEcommerceOrderService
{
    Task<IDataResults<ICollection<EcommerceOrderListDTO>>> GetOrdersByUserId(int id, string? orderGuid);
    // Task<IDataResults<ICollection<ShoppingOrderListDTO>>> GetOrderByOrderId(int id);
    Task<IDataResults<string>> CreateOrder(OrderDTO model);
}