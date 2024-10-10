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
using GTBack.Repository.Migrations;
using GTBack.Service.Utilities;
using GTBack.Service.Utilities.Jwt;
using GTBack.Service.Validation.Restourant;
using GTBack.Service.Validation.Tool;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using AE.Net.Mail;
using MimeKit;
using MailKit.Net.Smtp;  // This is the MailKit SmtpClient
using System;
using System.Threading.Tasks;

namespace GTBack.Service.Services.Ecommerce;

public class ClientAuthService : IAuthService
{
    private readonly IService<EcommerceClient> _service;
    private readonly IMailService _mailService;
    private readonly IService<EcommerceCompany> _companyService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ClaimsPrincipal? _loggedUser;
    private readonly IMapper _mapper;
    private readonly IJwtTokenService<BaseRegisterDTO> _tokenService;
    private readonly IMemoryCache _cache;
    private readonly IBackgroundJobClient _backgroundJobClient;



    public ClientAuthService(IMailService mailService, IService<EcommerceCompany> companyService, IMemoryCache cache, IRefreshTokenService refreshTokenService, IJwtTokenService<BaseRegisterDTO> tokenService,
        IHttpContextAccessor httpContextAccessor, IService<EcommerceClient> service,
         IMapper mapper)
    {
        _mapper = mapper;
        _companyService = companyService;
        _mailService = mailService;
        _cache = cache;
        _service = service;
        _loggedUser = httpContextAccessor.HttpContext?.User;
        _refreshTokenService = refreshTokenService;
        _tokenService = tokenService;

    }

    //GET CLİENT BY ID METHOD
    public async Task<IDataResults<UserDTO>> GetById(long id)
    {
        var place = await _service.GetByIdAsync(x => x.Id == id);
        var data = _mapper.Map<UserDTO>(place);
        return new SuccessDataResult<UserDTO>(data);
    }



    //DELETE CLİENT BY ID METHOD
    public async Task<IResults> Delete(long id)
    {
        var client = await _service.GetByIdAsync(x => x.Id == id);
        client.IsDeleted = true;
        await _service.UpdateAsync(client);
        return new SuccessResult();
    }



    ///Register CLİENT Method  we use validation TO DO:BATUHAN --> VALDATİONS DONE
    public async Task<IDataResults<AuthenticatedUserResponseDto>> Register(ClientRegisterRequestDTO registerDto)
    {
        var valResult =
            FluentValidationTool.ValidateModelWithKeyResult<ClientRegisterRequestDTO>(new ClientRegisterValidator(), registerDto);

        if (valResult.Success == false)
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>(HttpStatusCode.BadRequest, valResult.Errors);
        }

        var mail = registerDto.Mail.ToLower().Trim();
        var user = await _service.GetByIdAsync((x => x.Email.ToLower() == mail && !x.IsDeleted && x.EcommerceCompanyId == registerDto.CompanyId)); //get by mail eklenecek


        if (user != null)
        {
            valResult.Errors.Add("", Messages.User_Email_Exists);
            return new ErrorDataResults<AuthenticatedUserResponseDto>(HttpStatusCode.BadRequest, valResult.Errors);
        }


        user = new EcommerceClient()
        {
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            Email = registerDto.Mail,
            Address = registerDto.Address,
            Surname = registerDto.Surname,
            Phone = registerDto.Phone,
            IsDeleted = false,
            Name = registerDto.Name,
            EcommerceCompanyId = registerDto.CompanyId,
            PasswordHash = SHA1.Generate(registerDto.Password)
        };

        await _service.AddAsync(user);

        var response = await Authenticate(_mapper.Map<ClientRegisterRequestDTO>(user));
        return new SuccessDataResult<AuthenticatedUserResponseDto>(response, HttpStatusCode.OK);
    }

    public async Task<IDataResults<ClientUpdateDTO>> UpdateUser(ClientUpdateDTO registerDto)
    {
        var mail = registerDto.Mail.ToLower().Trim();
        //BURAYA FLUENT VALİDASYON YAPIALCAK
        // var valResult =
        //     FluentValidationTool.ValidateModelWithKeyResult<ClientRegisterRequestDTO>(new ClientRegisterValidator(), registerDto);

        var user = await _service.GetByIdAsync((x => x.Id == registerDto.Id && !x.IsDeleted)); //get by mail eklenecek
        user.UpdatedDate = DateTime.UtcNow;
        user.Email = user.Email;
        user.Address = registerDto.Address;
        user.Surname = registerDto.Surname;
        user.Phone = registerDto.Phone;
        user.Name = registerDto.Name;
        await _service.UpdateAsync(user);
        return new SuccessDataResult<ClientUpdateDTO>(registerDto, HttpStatusCode.OK);
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

    public async Task<IDataResults<ClientUpdateDTO>> Me()
    {
        var userId = GetLoggedUserId();

        ClientUpdateDTO? user = null;

        var userModel = await _service.FindAsNoTrackingAsync(x => !x.IsDeleted && x.Id == userId);
        user = _mapper.Map<ClientUpdateDTO>(userModel);


        if (user == null)
        {
            return new ErrorDataResults<ClientUpdateDTO>(Messages.User_NotFound_Message, HttpStatusCode.NotFound);
        }

        user.Mail = userModel!.Email;

        return new SuccessDataResult<ClientUpdateDTO>(user, "Succesful", HttpStatusCode.OK);
    }



    public async Task<IDataResults<AuthenticatedUserResponseDto>> Login(LoginDto loginDto, int companyId)
    {
        var valResult =
            FluentValidationTool.ValidateModelWithKeyResult(new ClientLoginValidator(), loginDto);
        if (valResult.Success == false)
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>(HttpStatusCode.BadRequest, valResult.Errors);
        }

        var mail = loginDto.Mail.ToLower().Trim();
        var parent = await _service.Where((x => x.Email.ToLower() == mail.ToLower() && x.EcommerceCompanyId == companyId && !x.IsDeleted)).FirstOrDefaultAsync(); //get by mail eklenecek


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

        var response = await Authenticate(_mapper.Map<ClientRegisterRequestDTO>(parent));
        return new SuccessDataResult<AuthenticatedUserResponseDto>(response);
    }

    private async Task<AuthenticatedUserResponseDto> Authenticate(ClientRegisterRequestDTO userDto)
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

    public async Task<IDataResults<AuthenticatedUserResponseDto>> GoogleLogin(GoogleLoginDTO model)
    {
        GoogleJsonWebSignature.ValidationSettings? settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>()
                { "1067621219285-fukuebsj13aa2b611b4fcs2j7s447kl6.apps.googleusercontent.com" }
        };
        GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken, settings);

        UserLoginInfo userLoginInfo = new(model.Provider, payload.Subject, model.Provider);

        var user = await _service.FindAsync(x => x.Email == payload.Email);
        bool result = user != null;


        if (!result)
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>(Messages.User_NotFound_Message,
                HttpStatusCode.BadRequest);
        }


        var response = await Authenticate(_mapper.Map<ClientRegisterRequestDTO>(user));
        return new SuccessDataResult<AuthenticatedUserResponseDto>(response);
    }
    public async Task<IResults> ResetPassword(ResetPasswordDTO password)
    {
        var user = await _service.Where(x => x.ActiveForgotLink == password.ActiveLink).FirstOrDefaultAsync();
        if (user == null)
        {
            return new ErrorResult("Süresi bitmiş yenileme link  veya  hiç böyle link var olmadı ");
        }

        user.PasswordHash = SHA1.Generate(password.NewPassword);
        user.ActiveForgotLink = "";
        await
            _service.UpdateAsync(user);

        return new SuccessResult("Şifre Başarıyla yenilendi");
    }

    public async Task<IResults> ResetPasswordLink(ResetPasswordLinkDTO resetPasswordLinkDto)
    {
        
        var user = _service.Where(x => x.Email == resetPasswordLinkDto.mail).FirstOrDefault();
        var company = _companyService.Where(x => x.Id == user.EcommerceCompanyId).FirstOrDefault();
        if (user == null)
        {
            return new ErrorResult("Bu e posta hesabı bir kullanıcıya ait değil");
        }
 string randomString = GenerateRandomString(40);
        string mailBody =
            "<!doctype html>\n<html lang=\"en-US\">\n\n<head>\n    <meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />\n    <title>Reset Password Email Template</title>\n    <meta name=\"description\" content=\"Reset Password Email Template.\">\n    <style type=\"text/css\">\n        a:hover {\n            text-decoration: underline !important;\n        }\n    </style>\n</head>\n\n<body marginheight=\"0\" topmargin=\"0\" marginwidth=\"0\" style=\"margin: 0px; background-color: #f2f3f8;\" leftmargin=\"0\">\n    <!--100% body table-->\n    <table cellspacing=\"0\" border=\"0\" cellpadding=\"0\" width=\"100%\" bgcolor=\"#f2f3f8\"\n        style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\">\n        <tr>\n            <td>\n                <table style=\"background-color: #f2f3f8; max-width:670px;  margin:0 auto;\" width=\"100%\" border=\"0\"\n                    align=\"center\" cellpadding=\"0\" cellspacing=\"0\">\n                    <tr>\n                        <td style=\"height:80px;\">&nbsp;</td>\n                    </tr>\n                    <tr>\n                        <td style=\"text-align:center;\">\n                            <a href=\"https://www." + company.WebAddress +"\" title=\"logo\" target=\"_blank\">\n                                <img width=\"150\" src=\"https://www.xn--boabutik-7fb.com/assets/images/logo-no-back.png\" title=\"logo\"\n                                    alt=\"logo\">\n                            </a>\n                        </td>\n                    </tr>\n                    <tr>\n                        <td style=\"height:20px;\">&nbsp;</td>\n                    </tr>\n                    <tr>\n                        <td>\n                            <table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"\n                                style=\"max-width:670px;background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);\">\n                                <tr>\n                                    <td style=\"height:40px;\">&nbsp;</td>\n                                </tr>\n                                <tr>\n                                    <td style=\"padding:0 35px;\">\n                                        <h1\n                                            style=\"color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;\">\n                                            Şifrenizi sıfırlama talebinde bulundunuz</h1>\n                                        <span\n                                            style=\"display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;\"></span>\n                                        <p style=\"color:#455056; font-size:15px;line-height:24px; margin:0;\">\n                                            Size eski şifrenizi öylece gönderemeyiz. Şifrenizi sıfırlamak için benzersiz\n                                            bir bağlantı sizin için oluşturuldu. Şifrenizi sıfırlamak için aşağıdaki\n                                            bağlantıya tıklayın ve talimatları izleyin.\n                                        </p>\n                                        <a href=\"https://www." + company.WebAddress +"/reset-password?key=" + randomString +"\"\n                                            style=\"background:#2d2e2d;text-decoration:none !important; font-weight:500; margin-top:35px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\">\n                                            Şifremi Yenile\n                                        </a>\n                                    </td>\n                                </tr>\n                                <tr>\n                                    <td style=\"height:40px;\">&nbsp;</td>\n                                </tr>\n                            </table>\n                        </td>\n                    <tr>\n                        <td style=\"height:20px;\">&nbsp;</td>\n                    </tr>\n                    <tr>\n                        <td style=\"text-align:center;\">\n                            <p\n                                style=\"font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;\">\n                                &copy; <strong>www." + company.WebAddress +"</strong></p>\n                        </td>\n                    </tr>\n                    <tr>\n                        <td style=\"height:80px;\">&nbsp;</td>\n                    </tr>\n                </table>\n            </td>\n        </tr>\n    </table>\n    <!--/100% body table-->\n</body>\n\n</html>";
        user.ActiveForgotLink = randomString;

        await _service.UpdateAsync(user);

        var mail = new MailServiceOptions()
        {
            SenderEmail = company.Email,
            ReceiverEmail = resetPasswordLinkDto.mail,
            ReceiverName = user.Name,
            Body = mailBody,
            Subject = "Şifreni mi unuttun",
            SenderName = company.Name,
            SmtpPort = (int)(company.SmtpPort.HasValue ? company.SmtpPort : 0),
            SmtpServer = company.SmtpServer,
            SenderPassword = company.EmailPassword,
        };

        _mailService.SendEmail(mail);
        return new SuccessResult();
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