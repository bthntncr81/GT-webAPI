using System.Security.Claims;
using AutoMapper;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Entities.Restourant;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Restourant;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GTBack.Service.Services.RestourantServices;

public class DepartmentService:IDepartmentService<DepartmentAddDTO,DepartmentListDTO>
{
    
    private readonly IService<Department> _service;
    private readonly ClaimsPrincipal? _loggedUser;
    private readonly IMapper _mapper;

    public DepartmentService(
        IHttpContextAccessor httpContextAccessor, IService<Department> service,
        IMapper mapper)
    {
        _mapper = mapper;
        _service = service;
        _loggedUser = httpContextAccessor.HttpContext?.User;
    }

    public  async Task<IDataResults<ICollection<DepartmentListDTO>>> ListByCompanyId(long companyId)
    {
        var depList =  _service.Where(x => !x.IsDeleted&&x.RestoCompanyId==companyId);
        var list = await depList.ToListAsync();
        var response = _mapper.Map<ICollection<DepartmentListDTO>>( list);
        return new SuccessDataResult<ICollection<DepartmentListDTO>>(response);    }

    public async Task<IDataResults<DepartmentAddDTO>> Create(DepartmentAddDTO model)
    {
        var response = _mapper.Map<Department>(model);
        await _service.AddAsync(response);
        return new SuccessDataResult<DepartmentAddDTO>(model);
        
    }

    public async Task<IDataResults<DepartmentAddDTO>> Update(DepartmentAddDTO model, long id)
    {
        throw new NotImplementedException();

    }

    public Task<IResults> Delete(long id)
    {
        
        throw new NotImplementedException();
    }

    public async Task<IDataResults<ICollection<DepartmentListDTO>>> List()
    {
        var depList =  _service.Where(x => !x.IsDeleted);
        var list = await depList.ToListAsync();
        var response = _mapper.Map<ICollection<DepartmentListDTO>>( list);
        return new SuccessDataResult<ICollection<DepartmentListDTO>>(response);
        
    }
}