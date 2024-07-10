using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Services.Shopping;
using Microsoft.AspNetCore.Mvc;

namespace GTBack.WebAPI.Controllers.Shopping;

public class ShoppingOrderController : CustomShoppingBaseController
{
    private readonly IShoppingOrderService _orderService;

    
    public ShoppingOrderController(IShoppingOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("ConfirmCheckout")]
    public async Task<IActionResult> AddProduct(OrderConfirmDTO model)
    {
            
        return ApiResult(await _orderService.CreateOrder(model));
    }
            
            
    [HttpGet("OrderByUserId")]
    public async Task<IActionResult> List([FromQuery]int userId)
    {
        return ApiResult(await _orderService.GetOrdersByUserId(userId));
    }

                
    [HttpGet("OrderbyOrderId")]
    public async Task<IActionResult> ListByOrderId([FromQuery]int orderId)
    {
        return ApiResult(await _orderService.GetOrderByOrderId(orderId));
    }

}