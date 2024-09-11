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

    
    private void SendMail(MailData mailData)
    {
        var smtpClient = new SmtpClient("smtp-mail.outlook.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential("kocumbenim_afl@hotmail.com", "Bthntncr81.")
        };
        


        var message = new MailMessage(mailData.SenderMail, mailData.RecieverMail, mailData.EmailSubject, mailData.EmailBody)
        {
            IsBodyHtml = true
        };

        smtpClient.Send(message);
    }
 public async  Task<IResults> CreateCoachGuid( )
    {
        var userIdClaim = _loggedUser.FindFirstValue("Id");

        string code = GenerateRandomString(10);

    var coach=  await  _coachService.Where(x => x.Id == long.Parse(userIdClaim)).FirstOrDefaultAsync();


    coach.ActiveCoachGuid = code;

    await _coachService.UpdateAsync(coach);
    string emailTemplate = $"<!doctype html><html lang=\"en-US\"><head><meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" /><title>Reset Password Email Template</title><meta name=\"description\" content=\"Reset Password Email Template.\"><style type=\"text/css\">a:hover {{text-decoration: underline !important;}}</style></head><body marginheight=\"0\" topmargin=\"0\" marginwidth=\"0\" style=\"margin: 0px; background-color: #f2f3f8;\" leftmargin=\"0\"><table cellspacing=\"0\" border=\"0\" cellpadding=\"0\" width=\"100%\" bgcolor=\"#f2f3f8\" style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\"><tr><td><table style=\"background-color: #f2f3f8; max-width:670px;margin:0 auto;\" width=\"100%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"><tr><td style=\"height:80px;\">&nbsp;</td></tr><tr><td style=\"text-align:center;\"><a href=\"https://www.boğabutik.com\" title=\"logo\" target=\"_blank\"><img width=\"150\" src=\"https://www.xn--boabutik-7fb.com/assets/images/logo-no-back.png\" title=\"logo\" alt=\"logo\"></a></td></tr><tr><td style=\"height:20px;\">&nbsp;</td></tr><tr><td><table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" style=\"max-width:670px;background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);\"><tr><td style=\"height:40px;\">&nbsp;</td></tr><tr><td style=\"padding:0 35px;\"><h1 style=\"color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;\">Öğretmen Kodu Talebinde Bulundunuz</h1><span style=\"display:inline-block; vertical-align:middle; margin:29px 0 26px;border-bottom:1px solid #cecece; width:100px;\"></span> Öğretmen Kodunuz {code} </td></tr><tr><td style=\"height:40px;\">&nbsp;</td></tr></table></td></table></td></tr></table></body></html>";
    var mail = new MailData()
    {
        SenderMail = "kocumbenim_afl@hotmail.com",
        RecieverMail = coach.Email,
        EmailBody =emailTemplate,
        EmailSubject = "Öğretmen Kayıt Kodu"
    };
        SendMail(mail);
        
        return new SuccessResult("Guid Sended");

    }
    
 
 static string GenerateRandomString(int length)
 {
     const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; // Define the character set
     Random random = new Random(); // Create a new instance of the Random class
     StringBuilder result = new StringBuilder(length);

     // Generate the random string
     for (int i = 0; i < length; i++)
     {
         // Get a random index in the range of the character set and append the character at that index to the result
         result.Append(chars[random.Next(chars.Length)]);
     }

     return result.ToString(); // Convert the StringBuilder to a string and return it
 }
    
}