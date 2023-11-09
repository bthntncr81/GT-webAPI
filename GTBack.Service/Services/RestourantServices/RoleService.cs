using System.Security.Claims;
using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Entities.Restourant;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Service.Utilities.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GTBack.Service.Services.RestourantServices;

public class RoleService : IRoleService<RoleCreateDTO,RoleListDTO>
{
    private readonly IService<Role> _service;
    private readonly ClaimsPrincipal? _loggedUser;
    private readonly IMapper _mapper;

    public RoleService(
        IHttpContextAccessor httpContextAccessor, IService<Role> service,
        IMapper mapper)
    {
        _mapper = mapper;
        _service = service;
        _loggedUser = httpContextAccessor.HttpContext?.User;
    }

    public async Task<IDataResults<RoleCreateDTO>> Create(RoleCreateDTO model)
    {
        var response = _mapper.Map<Role>(model);
        await _service.AddAsync(response);
        return new SuccessDataResult<RoleCreateDTO>(model);
    }

    public Task<IDataResults<RoleCreateDTO>> Update(RoleCreateDTO model, long id)
    {
        throw new NotImplementedException();
    }

    public Task<IResults> Delete(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<IDataResults<ICollection<RoleListDTO>>> List()
    {
        var roleList =  _service.Where(x => !x.IsDeleted);
        var list = await roleList.ToListAsync();
        var response = _mapper.Map<ICollection<RoleListDTO>>( list);
        return new SuccessDataResult<ICollection<RoleListDTO>>(response);

    }
}