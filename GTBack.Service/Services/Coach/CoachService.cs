using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using AutoMapper;
using Google.Apis.Util;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Coach.Request;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Entities;
using GTBack.Core.Entities.Coach;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.coach;
using GTBack.Service.Utilities;
using GTBack.Service.Utilities.Jwt;
using GTBack.Service.Validation.Tool;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Xml.Serialization;
using AutoMapper;
using Google.Apis.Auth;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Shopping;
using GTBack.Core.DTO.Shopping.Filter;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Entities;
using GTBack.Core.Entities.Ecommerce;
using GTBack.Core.Entities.Shopping;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Ecommerce;
using GTBack.Service.Utilities;
using GTBack.Service.Utilities.Jwt;
using GTBack.Service.Validation.Coach;
using GTBack.Service.Validation.Restourant;
using GTBack.Service.Validation.Tool;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public class CoachService : ICoachAuthService
{
    private readonly IService<Coach> _coachService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ClaimsPrincipal? _loggedUser;
    private readonly IMapper _mapper;
    private readonly IJwtTokenService<BaseRegisterDTO> _tokenService;
    private readonly IMemoryCache _cache;

    public CoachService(IMemoryCache cache, IRefreshTokenService refreshTokenService, IJwtTokenService<BaseRegisterDTO> tokenService,
        IHttpContextAccessor httpContextAccessor, IService<Coach> coachService, IMapper mapper)
    {
        _mapper = mapper;
        _cache = cache;
        _coachService = coachService;
        _loggedUser = httpContextAccessor.HttpContext?.User;
        _refreshTokenService = refreshTokenService;
        _tokenService = tokenService;
    }

    // Get Coach by ID
    public async Task<IDataResults<UserDTO>> GetById(long id)
    {
        var coach = await _coachService.GetByIdAsync(x => x.Id == id);
        var data = _mapper.Map<UserDTO>(coach);
        return new SuccessDataResult<UserDTO>(data);
    }

    // Register Coach
    public async Task<IDataResults<AuthenticatedUserResponseDto>> Register(CoachRegisterDTO registerDto)
    {
        var validationResult = FluentValidationTool.ValidateModelWithKeyResult(new CoachRegisterValidator(), registerDto);
        
        if (!validationResult.Success)
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>(HttpStatusCode.BadRequest, validationResult.Errors);
        }

        var email = registerDto.Mail.ToLower().Trim();
        var existingCoach = await _coachService.Where(x => x.Email.ToLower() == email && !x.IsDeleted).FirstOrDefaultAsync();

        if (existingCoach != null)
        {
            validationResult.Errors.Add("", "Email already exists");
            return new ErrorDataResults<AuthenticatedUserResponseDto>(HttpStatusCode.BadRequest, validationResult.Errors);
        }

        var coach = new Coach
        {
            Name = registerDto.Name,
            Surname = registerDto.Surname,
            Email = registerDto.Mail,
            Phone = registerDto.Phone,
            PasswordHash = SHA1.Generate(registerDto.Password),
            IsDeleted = false
        };

        await _coachService.AddAsync(coach);

        var response = await Authenticate(_mapper.Map<CoachRegisterDTO>(coach));
        return new SuccessDataResult<AuthenticatedUserResponseDto>(response, HttpStatusCode.OK);
    }

    // Update Coach
    public async Task<IDataResults<CoachUpdateDTO>> UpdateCoach(CoachUpdateDTO updateDto)
    {
        var coach = await _coachService.GetByIdAsync(x => x.Id == updateDto.Id && !x.IsDeleted);
        if (coach == null) return new ErrorDataResults<CoachUpdateDTO>("Coach not found");

        coach.Name = updateDto.Name;
        coach.Surname = updateDto.Surname;
        coach.Email = updateDto.Email;
        coach.Phone = updateDto.Phone;

        await _coachService.UpdateAsync(coach);
        return new SuccessDataResult<CoachUpdateDTO>(updateDto, HttpStatusCode.OK);
    }

    public Task<IDataResults<UserDTO>> Me()
    {
        throw new NotImplementedException();
    }

    // Delete Coach
    public async Task<IResults> Delete(long id)
    {
        var coach = await _coachService.GetByIdAsync(x => x.Id == id);
        if (coach == null) return new ErrorResult("Coach not found");

        coach.IsDeleted = true;
        await _coachService.UpdateAsync(coach);
        return new SuccessResult();
    }

    // Login
    public async Task<IDataResults<AuthenticatedUserResponseDto>> Login(LoginDto loginDto)
    {
       

        var email = loginDto.Mail.ToLower().Trim();
        var coach = await _coachService.Where(x => x.Email.ToLower() == email && !x.IsDeleted).FirstOrDefaultAsync();

        if (coach == null || SHA1.Verify(loginDto.Password, coach.PasswordHash))
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>("Invalid email or password", HttpStatusCode.BadRequest);
        }

        var response = await Authenticate(_mapper.Map<CoachRegisterDTO>(coach));
        return new SuccessDataResult<AuthenticatedUserResponseDto>(response);
    }

    // Authentication
    private async Task<AuthenticatedUserResponseDto> Authenticate(CoachRegisterDTO userDto)
    {
        var accessToken = _tokenService.GenerateAccessToken(userDto);
        var refreshToken = _tokenService.GenerateRefreshToken();

        await _refreshTokenService.Create(new RefreshTokenDto
        {
            Token = refreshToken,
        });

        return new AuthenticatedUserResponseDto
        {
            AccessToken = accessToken.Value,
            AccessTokenExpirationTime = accessToken.ExpirationTime,
            RefreshToken = refreshToken
        };
    }

    // Reset Password
    public async Task<IResults> ResetPassword(ResetPasswordDTO passwordDto)
    {
        var coach = await _coachService.Where(x => x.ActiveForgotLink == passwordDto.ActiveLink).FirstOrDefaultAsync();
        if (coach == null) return new ErrorResult("Invalid or expired link");

        coach.PasswordHash = SHA1.Generate(passwordDto.NewPassword);
        coach.ActiveForgotLink = string.Empty;
        await _coachService.UpdateAsync(coach);

        return new SuccessResult("Password reset successful");
    }

    public Task<IResults> ResetPasswordLink(ResetPasswordLinkDTO resetPasswordLinkDto)
    {
        throw new NotImplementedException();
    }
}