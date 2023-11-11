using System.Net;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Restourant;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;

namespace GTBack.WebAPI.Controllers.Restourant;

public class EmployeeController: CustomRestourantBaseController
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController( 
        IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
    
    [HttpPost("EmployeeList")]
    [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IDataResults<BaseListDTO<EmployeeListDTO,EmployeeFilterRepresent>>))]
    [ProducesResponseType(typeof(BaseListDTO<EmployeeListDTO,EmployeeFilterRepresent>),200)]
    public async Task<IActionResult> EmployeeList(BaseListFilterDTO<EmployeeListFilter> filter)
    {
        return ApiResult(await _employeeService.ListEmployee(filter));
    }

}