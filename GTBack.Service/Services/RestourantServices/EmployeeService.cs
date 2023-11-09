using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Entities;
using GTBack.Core.Entities.Restourant;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Restourant;
using GTBack.Service.Utilities;
using GTBack.Service.Utilities.Jwt;
using GTBack.Service.Validation.Restourant;
using GTBack.Service.Validation.Tool;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using XAct;

namespace GTBack.Service.Services.RestourantServices;

public class EmployeeService : IEmployeeService
{
    private readonly IService<Employee> _service;
    private readonly IMemoryCache _cache;
    private readonly IService<Department> _depService;
    private readonly IService<RestoCompany> _companyService;
    private readonly IService<EmployeeRoleRelation> _empRoleRelation;
    private readonly IService<Role> _roleService;
    private readonly IListingServiceI<Employee, EmployeeListFilter> _employeeListingService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ClaimsPrincipal? _loggedUser;
    private readonly IMapper _mapper;
    private readonly IJwtTokenService<BaseRegisterDTO> _tokenService;

    public EmployeeService(IRefreshTokenService refreshTokenService, IService<Department> depService,
        IService<Role> roleService, IJwtTokenService<BaseRegisterDTO> tokenService,
        IHttpContextAccessor httpContextAccessor, IService<Employee> service,
        IListingServiceI<Employee, EmployeeListFilter> employeeListingService,
        IMapper mapper, IMemoryCache cache, IService<RestoCompany> companyService,
        IService<EmployeeRoleRelation> empRoleRelation)
    {
        _mapper = mapper;
        _service = service;
        _depService = depService;
        _companyService = companyService;
        _roleService = roleService;
        _loggedUser = httpContextAccessor.HttpContext?.User;
        _employeeListingService = employeeListingService;
        _empRoleRelation = empRoleRelation;
        _refreshTokenService = refreshTokenService;
        _tokenService = tokenService;
        _cache = cache;
    }


    private async Task<AuthenticatedUserResponseDto> Authenticate(EmployeeRegisterDTO userDto)
    {
        var department = await _depService.Where(x => x.Id == userDto.DepartmentId).FirstOrDefaultAsync();
        var roleRel = await _empRoleRelation.Where(x => x.EmployeeId == userDto.Id).ToListAsync();
        var roleRes = _mapper.Map<ICollection<RoleList>>(roleRel);

        userDto.CompanyId = department.RestoCompanyId;

        userDto.RoleList = roleRes;
        var accessToken = _tokenService.GenerateAccessToken(userDto);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenDto = new RefreshTokenDto()
        {
            Token = refreshToken,
        };
        await _refreshTokenService.Create(refreshTokenDto);

        return new AuthenticatedUserResponseDto()
        {
            AccessToken = accessToken.Value,
            AccessTokenExpirationTime = accessToken.ExpirationTime,
            RefreshToken = refreshToken
        };
    }


    public Task<IDataResults<EmployeeListDTO>> GetById(int id)
    {
        throw new NotImplementedException();
    }


    public async Task<IDataResults<BaseListDTO<EmployeeListDTO, EmployeeFilterRepresent>>> ListEmployee(
        BaseListFilterDTO<EmployeeListFilter> filter)
    {
        var resource = await _cache.GetOrCreateAsync(_employeeListingService.CacheKey(filter), async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(2);
            var companyId = GetLoggedCompanyId();


            var employeeRepo = _service.Where(x => !x.IsDeleted);
            var companyRepo = _companyService.Where(x => !x.IsDeleted);
            var departmenRepo = _depService.Where(x => !x.IsDeleted);
            var roleRepo = _roleService.Where(x => !x.IsDeleted);
            var empRolRepo = _empRoleRelation.Where(x => !x.IsDeleted);

            var query = from department in departmenRepo.Where(x=>x.RestoCompanyId==companyId)
                join employee in employeeRepo on department.Id equals employee.DepartmentId into departmentLeft 
                from employee in departmentLeft.DefaultIfEmpty()
                select new EmployeeListDTO()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Surname = employee.Surname,
                    ShiftStart = employee.ShiftStart,
                    ShiftEnd = employee.ShiftEnd,
                    SalaryType = employee.SalaryType,
                    Salary = employee.Salary,
                    DeviceId = employee.DeviceId,
                    DepartmentId = employee.DepartmentId,
                    Address = employee.Address,
                    Phone = employee.Phone,
                    Mail = employee.Mail,
                    RoleList =  ( from empRolRel in empRolRepo.Where(x=>x.EmployeeId==employee.Id)
                        join role in roleRepo on empRolRel.RoleId equals role.Id into roleLeft
                        from role in roleLeft.DefaultIfEmpty()
                    select new RoleList()
                    {
                        RoleName = role.Name.IsNullOrEmpty() ? " ":  role.Name ,
                    }).ToList()
                };


            if (!(filter.RequestFilter.Name.IsNullOrEmpty()))
            {
                query = query.Where(x => x.Name.Equals(filter.RequestFilter.Name));
            }

            if (!filter.RequestFilter.Surname.IsNullOrEmpty())
            {
                query = query.Where(x => x.Surname.Equals(filter.RequestFilter.Surname));
            }

            if (!filter.RequestFilter.Mail.IsNullOrEmpty())
            {
                query = query.Where(x => x.Mail.Equals(filter.RequestFilter.Mail));
            }

            if (!filter.RequestFilter.Phone.IsNullOrEmpty())
            {
                query = query.Where(x => x.Phone.Equals(filter.RequestFilter.Phone));
            }

            if (filter.RequestFilter.Salary.HasValue)
            {
                query = query.Where(x => x.Salary == filter.RequestFilter.Salary);
            }

            if (filter.RequestFilter.SalaryType.HasValue)
            {
                query = query.Where(x => x.SalaryType == filter.RequestFilter.SalaryType);
            }

            if (filter.RequestFilter.DeviceId.HasValue)
            {
                query = query.Where(x => x.DeviceId == filter.RequestFilter.DeviceId);
            }

            if (!ObjectExtensions.IsNull(filter.RequestFilter.ShiftStart))
            {
                query = query.Where(x =>
                    x.ShiftStart > filter.RequestFilter.ShiftStart.StartDate &&
                    x.ShiftStart < filter.RequestFilter.ShiftStart.EndDate);
            }

            if (!ObjectExtensions.IsNull(filter.RequestFilter.ShiftEnd))
            {
                query = query.Where(x =>
                    x.ShiftEnd > filter.RequestFilter.ShiftStart.StartDate &&
                    x.ShiftEnd < filter.RequestFilter.ShiftStart.EndDate);
            }

            query = query.Skip(filter.PaginationFilter.Skip).Take(filter.PaginationFilter.Take);

            BaseListDTO<EmployeeListDTO, EmployeeFilterRepresent> baseList =
                new BaseListDTO<EmployeeListDTO, EmployeeFilterRepresent>();
            EmployeeFilterRepresent emp = new EmployeeFilterRepresent();
            baseList.Filter = emp;
            baseList.List = _mapper.Map<ICollection<EmployeeListDTO>>(await query.ToListAsync());
            return new SuccessDataResult<BaseListDTO<EmployeeListDTO, EmployeeFilterRepresent>>(baseList);
        });
        return resource;
    }

    public async Task<IDataResults<AuthenticatedUserResponseDto>> Login(LoginRestourantDTO loginDto)
    {
        var valResult =
            FluentValidationTool.ValidateModelWithKeyResult(new EmployeeLoginValidator(), loginDto);
        if (valResult.Success == false)
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>(HttpStatusCode.BadRequest, valResult.Errors);
        }

        var userName = loginDto.UserName.ToLower().Trim();
        var parent =
            await _service.GetByIdAsync((x => x.UserName.ToLower() == userName && !x.IsDeleted)); //get by mail eklenecek

     

        if (parent?.PasswordHash == null)
        {
            valResult.Errors.Add("", Messages.User_NotFound_Message);
            return new ErrorDataResults<AuthenticatedUserResponseDto>(Messages.User_NotFound_Message,
                HttpStatusCode.BadRequest);
        }

        var response = new AuthenticatedUserResponseDto();
        response.IsTemp = false;

        if (!Utilities.SHA1.Verify(loginDto.Password, parent.PasswordHash))
        {
            if (!Utilities.SHA1.Verify(loginDto.Password, parent.TempPasswordHash))
            {
                valResult.Errors.Add("", Messages.User_Login_Message_Notvalid);
                return new ErrorDataResults<AuthenticatedUserResponseDto>(Messages.Password_Wrong,
                    HttpStatusCode.BadRequest);
            }
            else
            {
                response = await Authenticate(_mapper.Map<EmployeeRegisterDTO>(parent));
                response.IsTemp = true;
            }
        }

        response = await Authenticate(_mapper.Map<EmployeeRegisterDTO>(parent));
        response.IsTemp = true;

        return new SuccessDataResult<AuthenticatedUserResponseDto>(response);
    }

    public long? GetLoggedUserId()
    {
        var userRoleString = _loggedUser.FindFirstValue("Id");
        if (long.TryParse(userRoleString, out var userId))
        {
            return userId;
        }

        return null;
    }

    public long? GetLoggedCompanyId()
    {
        var userRoleString = _loggedUser.FindFirstValue("companyId");
        if (long.TryParse(userRoleString, out var userId))
        {
            return userId;
        }

        return null;
    }

    public async Task<IDataResults<AuthenticatedUserResponseDto>> PasswordChoose(PasowordConfirmDTO passwordConfirmDto)
    {
        var id = GetLoggedUserId();

        var employee = await _service.GetByIdAsync((x => x.Id == id && !x.IsDeleted));

        employee.PasswordHash = SHA1.Generate(passwordConfirmDto.NewPassword);
        employee.TempPasswordHash = " ";

        await _service.UpdateAsync(employee);


        var response = await Authenticate(_mapper.Map<EmployeeRegisterDTO>(employee));

        return new SuccessDataResult<AuthenticatedUserResponseDto>(response);
    }

    public async Task<IResults> Register(EmployeeRegisterDTO registerDto)
    {
        var valResult =
            FluentValidationTool.ValidateModelWithKeyResult<EmployeeRegisterDTO>(new EmployeeRegisterValidator(),
                registerDto);

        if (valResult.Success == false)
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>(HttpStatusCode.BadRequest, valResult.Errors);
        }

        var mail = registerDto.Mail.ToLower().Trim();
        var employee =
            await _service.GetByIdAsync((x => x.Mail.ToLower() == mail && !x.IsDeleted)); //get by mail eklenecek


        if (!employee.IsNull())
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>(Messages.User_Username_Exist,
                HttpStatusCode.BadRequest);
        }
        
        var randomGenerator = new Random();
        var rndNum = randomGenerator.Next(10000000, 99999999);
        employee = new Employee()
        {
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            Mail = registerDto.Mail,
            Address = registerDto.Address,
            Surname = registerDto.Surname,
            DepartmentId = registerDto.DepartmentId,
            ShiftStart = registerDto.ShiftStart,
            ShiftEnd = registerDto.ShiftEnd,
            SalaryType = registerDto.SalaryType,
            Salary = registerDto.Salary,
            DeviceId = registerDto.DeviceId,
            Phone = registerDto.Phone,
            IsDeleted = false,
            Name = registerDto.Name,
            PasswordHash = ".",
            TempPasswordHash = SHA1.Generate(rndNum.ToString())
        };


        string mailBody = string.Format(
            "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n\n<head>\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge,chrome=1\">\n    <title>DR Levent Tuncer</title>\n</head>\n<style>\n    #button {{\n        display: flex;\n        align-items: center;\n        justify-content: center;\n        text-decoration: none;\n    }}\n\n    #button-wrapper {{\n        display: flex;\n        align-items: center;\n        justify-content: center;\n    }}\n\n    #title {{\n        text-align: center;\n    }}\n\n    .buttonContent {{\n        color: #FFFFFF;\n        font-family: Helvetica;\n        font-size: 18px;\n        font-weight: bold;\n        line-height: 100%;\n        padding: 15px;\n        text-align: center;\n    }}\n\n    .buttonContent a {{\n        color: #FFFFFF;\n        display: block;\n        text-decoration: none !important;\n        border: 0 !important;\n    }}\n</style>\n\n<body style=\"background-color: transparent;border-radius: 18px;\" leftmargin=\"0\" marginwidth=\"0\" topmargin=\"0\"\n    marginheight=\"0\" offset=\"0\">\n    <div style=\"padding: 20px;\">\n        <div style=\"border: 2px solid #52fa69;border-radius: 18px;\">\n            <div id=\"title\" style=\"height: 100px;width: 100%;background-color:#52fa69 ;display: flex;justify-content:\n                center;align-items: center;font-weight: bold;font-size: 36px;color: white;border-top-right-radius:\n                16px;border-top-left-radius: 16px;text-align: center;\"> Restoranium </div>\n            <div style=\"background-color: white;padding: 20px;border-bottom-right-radius:\n                16px;border-bottom-left-radius: 16px;\">\n                <div style=\"text-align: center;font-weight: bold;\">\n                    <p style=\"font-size: 28px;\">Geçici Şifreniz</p>\n                </div>\n                <div id=\"button-wrapper\" align=\"center\" valign=\"middle\" class=\"buttonContent\"\n                    style=\"padding-top:15px;padding-bottom:15px;padding-right:15px;padding-left:15px;\"> <a style=\"background-color: #52fa69;width: 200px;height:46px;border-radius:\n                        12px;color:#FFFFFF;text-decoration:none;font-family:Helvetica,Arial,sans-serif;font-size:20px;line-height:135%;padding:\n                        20px;\" href=\"https://drleventtuncerklinik.com\" target=\"_blank\">{0}\n                    </a> </div>\n            </div>\n        </div>\n    </div>\n</body>\n\n</html>",
            rndNum);

        var mailToSend = new MailData()
        {
            SenderMail = "batuhanntuncerr@hotmail.com",
            RecieverMail = employee.Mail,
            EmailSubject = "Geçici Şifre",
            EmailBody = mailBody
        };

        await _service.AddAsync(employee);
        foreach (var roleList in registerDto.RoleList)
        {
            var employeeRoleRelation = new EmployeeRoleRelation()
            {
                RoleId = roleList.RoleId,
                EmployeeId = employee.Id
            };

            await _empRoleRelation.AddAsync(employeeRoleRelation);
        }


        await _service.SendMail(mailToSend);
        return new SuccessResult();
    }

    public Task<IResults> EmployeeRoleChange(EmployeeRegisterDTO registerDto)
    {
        throw new NotImplementedException();
    }
}