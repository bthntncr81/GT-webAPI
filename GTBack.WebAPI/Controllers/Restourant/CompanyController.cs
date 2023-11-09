using AutoMapper;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Services;
using GTBack.Core.Services.Restourant;
using Microsoft.AspNetCore.Mvc;

namespace GTBack.WebAPI.Controllers.Restourant;

public class CompanyController: CustomRestourantBaseController
{
    private readonly IMapper _mapper;
    private readonly IRestoCompanyService<CompanyAddDTO,CompanyListDTO> _companyService;

    public CompanyController( IMapper mapper,IRestoCompanyService<CompanyAddDTO,CompanyListDTO> companyService)
    {
        _companyService = companyService;
        _mapper = mapper;
    }
    
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CompanyAddDTO role)
    {
        return ApiResult(await _companyService.Create(role));
    }
}