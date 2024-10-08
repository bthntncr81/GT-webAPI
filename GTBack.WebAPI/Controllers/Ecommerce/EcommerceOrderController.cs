using GTBack.Core.DTO;
using GTBack.Core.DTO.Ecommerce;
using GTBack.Core.DTO.Ecommerce.Request;
using GTBack.Core.DTO.Shopping.Filter;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Services.Ecommerce;
using GTBack.Core.Services.Shopping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GTBack.WebAPI.Controllers.Ecommerce;

public class EcommerceOrderController : CustomEcommerceBaseController
{
    private readonly IEcommerceOrderService _orderService;


    public EcommerceOrderController(IEcommerceOrderService orderService)
    {
        _orderService = orderService;
    }

    [Microsoft.AspNetCore.Mvc.HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrder(OrderDTO model)
    {

        return ApiResult(await _orderService.CreateOrder(model));
    }

    [Microsoft.AspNetCore.Mvc.HttpGet("GetOrdersByUserId/{id}/{orderId}/")]
    public async Task<IActionResult> GetOrdersByUserId(int id, int? orderId)
    {
        return ApiResult(await _orderService.GetOrdersByUserId(id, orderId));
    }




}