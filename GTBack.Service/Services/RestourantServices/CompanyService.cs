using System.Security.Claims;
using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Entities.Restourant;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Restourant;
using GTBack.Service.Utilities;
using GTBack.Service.Utilities.Jwt;
using Microsoft.AspNetCore.Http;

namespace GTBack.Service.Services.RestourantServices;

public class CompanyService : IRestoCompanyService<CompanyAddDTO,CompanyListDTO>
{
    private readonly IService<RestoCompany> _companyService;
    private readonly IService<Department> _departmentService;
    private readonly IService<Employee> _employeeService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IMenuAndCategoryService _menuService;
    private readonly ClaimsPrincipal? _loggedUser;
    private readonly IMapper _mapper;
    private readonly IJwtTokenService<BaseRegisterDTO> _tokenService;

    public CompanyService(IRefreshTokenService refreshTokenService, IService<Employee> employeeService,
        IService<Department> departmentService, IJwtTokenService<BaseRegisterDTO> tokenService,
        IHttpContextAccessor httpContextAccessor, IService<RestoCompany> companyService,IMenuAndCategoryService menuService,
        IMapper mapper)
    {
        _mapper = mapper;
        _companyService = companyService;
        _departmentService = departmentService;
        _employeeService = employeeService;
        _loggedUser = httpContextAccessor.HttpContext?.User;
        _refreshTokenService = refreshTokenService;
        _tokenService = tokenService;
        _menuService = menuService;
    }

    public async Task<IDataResults<CompanyAddDTO>> Create(CompanyAddDTO model)
    {
     
        var addedCompany = new RestoCompany
        {
            Name = model.CompanyName,
            Address = model.CompanyAddress,
            Mail = model.CompanyMail,
            Phone = model.CompanyPhone,
            Lat = model.Lat.HasValue ? model.Lat : 0,
            Lng = model.Lng.HasValue ? model.Lng : 0,
        };
        
      

        var company = await _companyService.AddAsync(addedCompany);
        
        var menuAdded = new MenuCreateDTO()
        {
            Name = model.CompanyName,
            RestoCompanyId = company.Id
        };
        
         await  _menuService.MenuCreate(menuAdded);

        var depAdded = new Department()
        {
            Name = "Manager",
            Mail = company.Mail,
            Phone = model.CompanyPhone,
            RestoCompanyId = company.Id,
        };

        var department = await _departmentService.AddAsync(depAdded);
        var employeeAdded = new Employee()
        {
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            Mail = model.Mail,
            Address = model.Address,
            Surname = model.Surname,
            DepartmentId = department.Id,
            ShiftStart = model.ShiftStart,
            ShiftEnd = model.ShiftEnd,
            SalaryType = model.SalaryType,
            Salary = model.Salary,
            Phone = model.Phone,
            IsDeleted = false,
            Name = model.Name,
            PasswordHash = SHA1.Generate(model.Password)
        };

        var user = await _employeeService.AddAsync(employeeAdded);

        return new SuccessDataResult<CompanyAddDTO>(model);
    }

    public Task<IDataResults<CompanyAddDTO>> Update(CompanyAddDTO model, long id)
    {
        throw new NotImplementedException();
    }

    public Task<IResults> Delete(long id)
    {
        throw new NotImplementedException();
    }

    public Task<IDataResults<ICollection<CompanyListDTO>>> List()
    {
        throw new NotImplementedException();
    }
}