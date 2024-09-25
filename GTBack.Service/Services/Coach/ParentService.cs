using System.Net;
using System.Security.Claims;
using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Coach.Request;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Entities.Coach;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.coach;
using GTBack.Service.Utilities;
using GTBack.Service.Utilities.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;


public class ParentService :IParentAuthService
{
    private readonly IService<Parent> _parentService;
    private readonly IService<Student> _studentService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IMapper _mapper;
    private readonly IJwtTokenService<BaseRegisterDTO> _tokenService;

    public ParentService(IService<Student> studentService, IRefreshTokenService refreshTokenService, IJwtTokenService<BaseRegisterDTO> tokenService,
        IService<Parent> parentService, IMapper mapper)
    {
        _mapper = mapper;
        _parentService = parentService;
        _refreshTokenService = refreshTokenService;
        _studentService = studentService;
        _tokenService = tokenService;
    }

    // Get Coach by ID
    public async Task<IDataResults<UserDTO>> GetById(long id)
    {
        var coach = await _parentService.GetByIdAsync(x => x.Id == id);
        var data = _mapper.Map<UserDTO>(coach);
        return new SuccessDataResult<UserDTO>(data);
    }

    // Register Coach
    public async Task<IDataResults<AuthenticatedUserResponseDto>> Register(ParentRegisterDTO registerDto)
    {
        // var validationResult = FluentValidationTool.ValidateModelWithKeyResult(new CoachRegisterValidator(), registerDto);
        //
        // if (!validationResult.Success)
        // {
        //     return new ErrorDataResults<AuthenticatedUserResponseDto>(HttpStatusCode.BadRequest, validationResult.Errors);
        // }

        var email = registerDto.Mail.ToLower().Trim();
        var student = await _studentService.Where(x => x.Email.ToLower() == email.ToLower() && !x.IsDeleted).FirstOrDefaultAsync();
        var existingParent = await _parentService.Where(x => x.Email.ToLower() == email.ToLower() && !x.IsDeleted).FirstOrDefaultAsync();

        if (existingParent != null)
        {
            // validationResult.Errors.Add("", "Email already exists");
            return new ErrorDataResults<AuthenticatedUserResponseDto>( "Bu emailde kullanıcı var",HttpStatusCode.BadRequest);
        }
        
        if (student.ParentId != null)
        {
            // validationResult.Errors.Add("", "Email already exists");
            return new ErrorDataResults<AuthenticatedUserResponseDto>( "Bu  öğrencinin hali hazırda bir velisi var",HttpStatusCode.BadRequest);
        }

        var coach = new Parent
        {
            Name = registerDto.Name,
            Surname = registerDto.Surname,
            Email = registerDto.Mail,
            Phone = registerDto.Phone,
            InitialPassword = registerDto.InitialPassword,
            StudentId = student.Id,
            PasswordHash = SHA1.Generate(registerDto.Password),
            IsDeleted = false
        };

        await _parentService.AddAsync(coach);

        var response = await Authenticate(_mapper.Map<ParentRegisterDTO>(coach));
        return new SuccessDataResult<AuthenticatedUserResponseDto>(response, HttpStatusCode.OK);
    }

    // Update Coach
    public async Task<IDataResults<CoachUpdateDTO>> UpdateCoach(CoachUpdateDTO updateDto)
    {
        var coach = await _parentService.GetByIdAsync(x => x.Id == updateDto.Id && !x.IsDeleted);
        if (coach == null) return new ErrorDataResults<CoachUpdateDTO>("Coach not found");

        coach.Name = updateDto.Name;
        coach.Surname = updateDto.Surname;
        coach.Email = updateDto.Email;
        coach.Phone = updateDto.Phone;

        await _parentService.UpdateAsync(coach);
        return new SuccessDataResult<CoachUpdateDTO>(updateDto, HttpStatusCode.OK);
    }

    public Task<IDataResults<UserDTO>> Me()
    {
        throw new NotImplementedException();
    }

    // Delete Coach
    public async Task<IResults> Delete(long id)
    {
        var coach = await _parentService.GetByIdAsync(x => x.Id == id);
        if (coach == null) return new ErrorResult("Coach not found");

        coach.IsDeleted = true;
        await _parentService.UpdateAsync(coach);
        return new SuccessResult();
    }

    // Login
    public async Task<IDataResults<AuthenticatedUserResponseDto>> Login(LoginDto loginDto)
    {
       

        var email = loginDto.Mail.ToLower().Trim();
        var coach = await _parentService.Where(x => x.Email.ToLower() == email && !x.IsDeleted).FirstOrDefaultAsync();

        if (coach == null || SHA1.Verify(loginDto.Password, coach.PasswordHash))
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>("Invalid email or password", HttpStatusCode.BadRequest);
        }

        var response = await Authenticate(_mapper.Map<ParentRegisterDTO>(coach));
        return new SuccessDataResult<AuthenticatedUserResponseDto>(response);
    }

    // Authentication
    private async Task<AuthenticatedUserResponseDto> Authenticate(ParentRegisterDTO userDto)
    {
        var accessToken = _tokenService.GenerateAccessTokenParent(userDto,"parent",userDto.InitialPassword);
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
        var coach = await _parentService.Where(x => x.ActiveForgotLink == passwordDto.ActiveLink).FirstOrDefaultAsync();
        if (coach == null) return new ErrorResult("Invalid or expired link");

        coach.PasswordHash = SHA1.Generate(passwordDto.NewPassword);
        coach.ActiveForgotLink = string.Empty;
        await _parentService.UpdateAsync(coach);

        return new SuccessResult("Password reset successful");
    }

    public Task<IResults> ResetPasswordLink(ResetPasswordLinkDTO resetPasswordLinkDto)
    {
        throw new NotImplementedException();
    }

    


    
 

    
}