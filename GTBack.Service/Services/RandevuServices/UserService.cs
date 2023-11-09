using System.Net;
using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using Google.Apis.Auth;
using GTBack.Core.DTO;
using GTBack.Core.Entities;
using GTBack.Core.Enums;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.UnitOfWorks;
using GTBack.Service.Utilities;
using GTBack.Service.Utilities.Jwt;
using GTBack.Service.Validation;
using GTBack.Service.Validation.Tool;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GTBack.Service.Services;

public class UserService : IUserService
{
    private readonly IService<User> _service;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ClaimsPrincipal? _loggedUser;
    private readonly IMapper _mapper;
    private readonly IValidatorFactory _validatorFactory;
    private readonly IJwtTokenService<BaseRegisterDTO>_tokenService;

    public UserService(IRefreshTokenService refreshTokenService, IJwtTokenService<BaseRegisterDTO>tokenService,
        IValidatorFactory validatorFactory, IHttpContextAccessor httpContextAccessor, IService<User> service,
         IMapper mapper)
    {
        _mapper = mapper;
        _service = service;
        _loggedUser = httpContextAccessor.HttpContext?.User;
        _validatorFactory = validatorFactory;
        _refreshTokenService = refreshTokenService;
        _tokenService = tokenService;
    }


    public async Task<IDataResults<UserDTO>> GetById(int id)
    {
        var place = await _service.GetByIdAsync(x => x.Id == id);
        var data = _mapper.Map<UserDTO>(place);
        return new SuccessDataResult<UserDTO>(data);
    }


    public async Task<IResults> Delete(int id)
    {
        var place = await _service.GetByIdAsync(x => x.Id == id);
        place.IsDeleted = true;
        await _service.UpdateAsync(place);
        return new SuccessResult();
    }

    public async Task<IDataResults<AuthenticatedUserResponseDto>> Register(UserRegisterDTO registerDto)
    {
        var valResult =
            FluentValidationTool.ValidateModelWithKeyResult<UserRegisterDTO>(new CustomerDtoValidator(), registerDto);
        if (valResult.Success == false)
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>(HttpStatusCode.BadRequest, valResult.Errors);
        }

        var mail = registerDto.Mail.ToLower().Trim();
        var user = await _service.GetByIdAsync((x => x.Mail.ToLower() == mail && !x.IsDeleted)); //get by mail eklenecek


        if (user != null)
        {
            valResult.Errors.Add("", Messages.User_Email_Exists);
            return new ErrorDataResults<AuthenticatedUserResponseDto>(HttpStatusCode.BadRequest, valResult.Errors);
        }


        user = new User()
        {
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            Mail = registerDto.Mail,
            UserTypeId = registerDto.UserTypeId,
            Address = registerDto.Address,
            Surname = registerDto.Surname,
            Phone = registerDto.Phone,
            IsDeleted = false,
            Name = registerDto.Name,
            CompanyId = registerDto.CompanyId != 0 ? registerDto.CompanyId : null,
            PasswordHash = SHA1.Generate(registerDto.Password)
        };

        await _service.AddAsync(user);

        var response = await Authenticate(_mapper.Map<UserRegisterDTO>(user));
        return new SuccessDataResult<AuthenticatedUserResponseDto>(response, HttpStatusCode.OK);
    }

    //
    // public async Task<IDataResults<ICollection<UserDTO>>> List(CustomerListParameters parameters)
    // {
    //     var query = _service
    //         .Where(x => !x.IsDeleted)
    //         .AsNoTracking();
    //
    //     switch (parameters.Order)
    //     {
    //         case ListOrderType.Ascending:
    //             query = query.OrderBy(o => o.Id);
    //             break;
    //         case ListOrderType.Descending:
    //             query = query.OrderByDescending(o => o.Id);
    //             break;
    //     }
    //
    //
    //     if (parameters.customerId.HasValue)
    //     {
    //         query = query.Where(x => x.Id == parameters.customerId.Value);
    //     }
    //
    //     var totalCount = await query.CountAsync();
    //
    //     if (parameters.Skip.HasValue)
    //     {
    //         query = query.Skip(parameters.Skip.Value);
    //     }
    //
    //     if (parameters.Take.HasValue)
    //     {
    //         query = query.Take(parameters.Take.Value);
    //     }
    //     if (parameters.Search != null)
    //     {
    //         query = query.Where(x => x.Name == parameters.Search);
    //     }
    //    
    //     var data = _mapper.Map<ICollection<UserDTO>>(await query.ToListAsync());
    //     return new SuccessDataResult<ICollection<UserDTO>>(data, totalCount);
    // }
    public int? GetLoggedUserId()
    {
        var userRoleString = _loggedUser.FindFirstValue("Id");
        if (int.TryParse(userRoleString, out var userId))
        {
            return userId;
        }

        return null;
    }

    public async Task<IDataResults<UserDTO>> Me()
    {
        var userId = GetLoggedUserId();

        UserDTO? user = null;


        var userModel = await _service.FindAsNoTrackingAsync(x => !x.IsDeleted && x.Id == userId);
        user = _mapper.Map<UserDTO>(userModel);


        if (user == null)
        {
            return new ErrorDataResults<UserDTO>(Messages.User_NotFound_Message, HttpStatusCode.NotFound);
        }

        return new SuccessDataResult<UserDTO>(user);
    }

    public async Task<IDataResults<ICollection<UserForDropdownDTO>>> AdminListByCompanyId(int companyId)
    {
        var userModel = _service.Where(x => !x.IsDeleted && x.CompanyId == companyId&&x.UserTypeId==1);
        var user = _mapper.Map<ICollection<UserForDropdownDTO>>(await userModel.ToListAsync());

        return new SuccessDataResult<ICollection<UserForDropdownDTO>>(user, user.Count);
    }

    public async Task<IDataResults<AuthenticatedUserResponseDto>> Login(LoginDto loginDto)
    {
        var valResult =
            FluentValidationTool.ValidateModelWithKeyResult(_validatorFactory.GetValidator<LoginDto>(), loginDto);
        if (valResult.Success == false)
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>(HttpStatusCode.BadRequest, valResult.Errors);
        }

        var mail = loginDto.Mail.ToLower().Trim();
        var parent =
            await _service.GetByIdAsync((x => x.Mail.ToLower() == mail && !x.IsDeleted)); //get by mail eklenecek


        if (parent?.PasswordHash == null)
        {
            valResult.Errors.Add("", Messages.User_NotFound_Message);
            return new ErrorDataResults<AuthenticatedUserResponseDto>(Messages.User_NotFound_Message,
                HttpStatusCode.BadRequest);
        }

        if (!Utilities.SHA1.Verify(loginDto.Password, parent.PasswordHash))
        {
            valResult.Errors.Add("", Messages.User_Login_Message_Notvalid);
            return new ErrorDataResults<AuthenticatedUserResponseDto>(Messages.Password_Wrong,
                HttpStatusCode.BadRequest);
        }

        var response = await Authenticate(_mapper.Map<UserRegisterDTO>(parent));
        return new SuccessDataResult<AuthenticatedUserResponseDto>(response);
    }

    private async Task<AuthenticatedUserResponseDto> Authenticate(UserRegisterDTO userDto)
    {
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


    public async Task<IDataResults<AuthenticatedUserResponseDto>> GoogleLogin (GoogleLoginDTO model)
    {
        GoogleJsonWebSignature.ValidationSettings? settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>()
                { "1067621219285-fukuebsj13aa2b611b4fcs2j7s447kl6.apps.googleusercontent.com" }
        };
        GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken, settings);

        UserLoginInfo userLoginInfo = new(model.Provider, payload.Subject, model.Provider);

        var user = await _service.FindAsync(x => x.Mail == payload.Email);
        bool result = user != null;


        if (!result)
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>(Messages.User_NotFound_Message,
                HttpStatusCode.BadRequest);
        }
        

        var response = await Authenticate(_mapper.Map<UserRegisterDTO>(user));
        return new SuccessDataResult<AuthenticatedUserResponseDto>(response);
    }
}