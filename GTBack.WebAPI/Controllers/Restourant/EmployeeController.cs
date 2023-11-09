using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Services;
using GTBack.Core.Services.Restourant;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> EmployeeList(BaseListFilterDTO<EmployeeListFilter> filter)
    {
        return ApiResult(await _employeeService.ListEmployee(filter));
    }

}