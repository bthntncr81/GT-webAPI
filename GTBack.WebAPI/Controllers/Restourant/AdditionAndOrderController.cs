using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.Entities.Restourant;
using GTBack.Core.Enums.Restourant;
using GTBack.Core.Services;
using GTBack.Core.Services.Restourant;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace GTBack.WebAPI.Controllers.Restourant;

public class AdditionAndOrderController: CustomRestourantBaseController
{
    private readonly IMapper _mapper;
    private readonly IService<ExtraMenuItem> _extraMenuService;
    private readonly IAdditionAndOrderService _service;

    public AdditionAndOrderController(IService<ExtraMenuItem> extraMenuService,IBackgroundJobClient backgroundJobClient, IMapper mapper,IAdditionAndOrderService service)
    {
        _service = service;
        _mapper = mapper;
        _extraMenuService = extraMenuService;
    }
    
    [HttpPost("AdditionAddOrUpdate")]
    public async Task<IActionResult> AdditionAddOrUpdate(AdditionAddOrUpdateDTO model)
    {
        return ApiResult(await _service.AdditionAddOrUpdate(model));
    }
    
   
    
    [HttpPost("OrderAddOrUpdate")]
    public async Task<IActionResult> OrderAddOrUpdate(OrderAddOrUpdateDTO model)
    {
        return ApiResult(await _service.OrderAddOrUpdate(model));
    }
    

  
    
    [HttpPost("ChangeOrderStatus")]
    public async Task<IActionResult> ChangeOrderStatus(ChangeOrderStatusDTO model)
    {
        return ApiResult(await _service.ChangeOrderStatus(model));
    }
    
    [HttpPost("CloseAddition")]
    public async Task<IActionResult> CloseAddition([FromQuery]long additionId)
    {
        return ApiResult(await _service.CloseAddition(additionId));
    }
    
    [HttpPost("AllAdditionList")]
    public async Task<IActionResult> AllAdditionList(BaseListFilterDTO<AdditionFilterDTO> filter)
    {
        return ApiResult(await _service.AllAdditionList(filter));
    }
    
    
    
    [HttpPost("OrderListByAdditionId")]
    public async Task<IActionResult> OrderListByAdditionId(BaseListFilterDTO<OrderFilterDTO> filter, long additionId)
    {
        return ApiResult(await _service.OrderListByAdditionId(filter,additionId));
    }
    

   
    
    [HttpDelete("AdditionDelete")]
    public async Task<IActionResult> AdditionDelete(int id)
    {
        return ApiResult(await _service.AdditionDelete(id));
    }
    
    [HttpDelete("OrderDelete")]
    public async Task<IActionResult> OrderDelete(int id)
    {
        return ApiResult(await _service.OrderDelete(id));
    }
}