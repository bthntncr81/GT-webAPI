using AutoMapper;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Services;
using GTBack.Core.Services.Restourant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GTBack.WebAPI.Controllers.Restourant;

public class TableAndTableAreaController: CustomRestourantBaseController
{
    private readonly IMapper _mapper;
    private readonly ITableAndAreaService _service;

    public TableAndTableAreaController( IMapper mapper,ITableAndAreaService service)
    {
        _service = service;
        _mapper = mapper;
    }
    
         
    [HttpPost("TableAreaAddOrUpdate")]
    public async Task<IActionResult> TableAreaAddOrUpdate(TableAreaAddOrUpdateDTO model)
    {
        return ApiResult(await _service.TableAreaAddOrUpdate(model));
    }
    
   
    
    [HttpPost("TableAddOrUpdate")]
    public async Task<IActionResult> TableAddOrUpdate(TableAddOrUpdateDTO model)
    {
        return ApiResult(await _service.TableAddOrUpdate(model));
    }
    
    [HttpDelete("TableAreaDelete")]
    public async Task<IActionResult> TableAreaDelete(long id)
    {
        return ApiResult(await _service.TableAreaDelete(id));
    }
    
    [HttpDelete("TableDelete")]
    public async Task<IActionResult> TableDelete(long id)
    {
        return ApiResult(await _service.TableDelete(id));
    }
    
  
    [Authorize]
    [HttpGet("TableAreaListByCompanyId")]
    public async Task<IActionResult> TableAreaListByCompanyId()
    {
        return ApiResult(await _service.TableAreaListByCompanyId());
    }
    
    [HttpGet("TableListTableAreaId")]
    public async Task<IActionResult> TableListTableAreaId(long tableAreaId)
    {
        return ApiResult(await _service.TableListTableAreaId(tableAreaId));
    }
    [Authorize]
    [HttpGet("AllTableList")]
    public async Task<IActionResult> AllTableList()
    {
        return ApiResult(await _service.AllTableList());
    }
    
}