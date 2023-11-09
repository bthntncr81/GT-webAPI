using AutoMapper;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Services;
using GTBack.Core.Services.Restourant;
using Microsoft.AspNetCore.Mvc;

namespace GTBack.WebAPI.Controllers.Restourant;

public class DepartmentController: CustomRestourantBaseController
{
    private readonly IMapper _mapper;
    private readonly IDepartmentService<DepartmentAddDTO,DepartmentListDTO> _service;

    public DepartmentController( IMapper mapper,IDepartmentService<DepartmentAddDTO,DepartmentListDTO> service,IEmployeeService employeeService)
    {
        _service = service;
        _mapper = mapper;
    }
    
         
    [HttpPost("Create")]
    public async Task<IActionResult> Create(DepartmentAddDTO department)
    {
        return ApiResult(await _service.Create(department));
    }
    
    [HttpPost("ListByCompanyId")]
    public async Task<IActionResult> ListByCompanyId(long companyId)
    {
        return ApiResult(await _service.ListByCompanyId(companyId));
    }
    
    [HttpPost("AllList")]
    public async Task<IActionResult> AllList()
    {
        return ApiResult(await _service.List());
    }
}