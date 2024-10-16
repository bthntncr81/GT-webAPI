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


public class ParentService : IParentAuthService
{
    private readonly IService<Parent> _parentService;
    private readonly IService<Student> _studentService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IMapper _mapper;
    private readonly IJwtTokenService<BaseRegisterDTO> _tokenService;
    private readonly IMailService _mailService;

    public ParentService(IMailService mailService, IService<Student> studentService, IRefreshTokenService refreshTokenService, IJwtTokenService<BaseRegisterDTO> tokenService,
        IService<Parent> parentService, IMapper mapper)
    {
        _mapper = mapper;
        _mailService = mailService;
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

        var email = registerDto.StudentMail.ToLower().Trim();
        var student = await _studentService.Where(x => x.Email.ToLower() == email.ToLower() && !x.IsDeleted).FirstOrDefaultAsync();
        var existingParent = await _parentService.Where(x => x.Email.ToLower() == registerDto.Mail.ToLower() && !x.IsDeleted).FirstOrDefaultAsync();

        if (existingParent != null)
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>("Bu emailde kullanıcı var", HttpStatusCode.BadRequest);
        }
        else if (student == null)
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>("Böyle bir öğrenci yok", HttpStatusCode.BadRequest);
        }
        else if (student.ParentId != null && student.ParentId != 0)
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>("Bu  öğrencinin hali hazırda bir velisi var", HttpStatusCode.BadRequest);
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

        var parent = await _parentService.AddAsync(coach);

        student.ParentId = parent.Id;

        await _studentService.UpdateAsync(student);

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

        if (coach == null || !SHA1.Verify(loginDto.Password, coach.PasswordHash))
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>("Invalid email or password", HttpStatusCode.BadRequest);
        }

        var response = await Authenticate(_mapper.Map<ParentRegisterDTO>(coach));
        return new SuccessDataResult<AuthenticatedUserResponseDto>(response);
    }

    // Authentication
    private async Task<AuthenticatedUserResponseDto> Authenticate(ParentRegisterDTO userDto)
    {
        var accessToken = _tokenService.GenerateAccessTokenParent(userDto, "parent", userDto.InitialPassword);
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

    public async Task<IResults> ResetPasswordLink(ResetPasswordLinkDTO resetPasswordLinkDto)
    {
        string randomString = GenerateRandomString(40);
        string mailBody =
            "<!doctype html>\n<html lang=\"en-US\">\n\n<head>\n    <meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />\n    <title>Reset Password Email Template</title>\n    <meta name=\"description\" content=\"Reset Password Email Template.\">\n    <style type=\"text/css\">\n        a:hover {\n            text-decoration: underline !important;\n        }\n    </style>\n</head>\n\n<body marginheight=\"0\" topmargin=\"0\" marginwidth=\"0\" style=\"margin: 0px; background-color: #f2f3f8;\" leftmargin=\"0\">\n    <!--100% body table-->\n    <table cellspacing=\"0\" border=\"0\" cellpadding=\"0\" width=\"100%\" bgcolor=\"#f2f3f8\"\n        style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\">\n        <tr>\n            <td>\n                <table style=\"background-color: #f2f3f8; max-width:670px;  margin:0 auto;\" width=\"100%\" border=\"0\"\n                    align=\"center\" cellpadding=\"0\" cellspacing=\"0\">\n                    <tr>\n                        <td style=\"height:80px;\">&nbsp;</td>\n                    </tr>\n                    <tr>\n                        <td style=\"text-align:center;\">\n                            <a href=\"https://www.aoflots.com\" title=\"logo\" target=\"_blank\">\n                                <img width=\"150\" src=\"https://www.aoflots.com/assets/logo-blue.png\" title=\"logo\"\n                                    alt=\"logo\">\n                            </a>\n                        </td>\n                    </tr>\n                    <tr>\n                        <td style=\"height:20px;\">&nbsp;</td>\n                    </tr>\n                    <tr>\n                        <td>\n                            <table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"\n                                style=\"max-width:670px;background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);\">\n                                <tr>\n                                    <td style=\"height:40px;\">&nbsp;</td>\n                                </tr>\n                                <tr>\n                                    <td style=\"padding:0 35px;\">\n                                        <h1\n                                            style=\"color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;\">\n                                            Şifrenizi sıfırlama talebinde bulundunuz</h1>\n                                        <span\n                                            style=\"display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;\"></span>\n                                        <p style=\"color:#455056; font-size:15px;line-height:24px; margin:0;\">\n                                            Size eski şifrenizi öylece gönderemeyiz. Şifrenizi sıfırlamak için benzersiz\n                                            bir bağlantı sizin için oluşturuldu. Şifrenizi sıfırlamak için aşağıdaki\n                                            bağlantıya tıklayın ve talimatları izleyin.\n                                        </p>\n                                        <a href=\"https://www.aoflots.com/auth/reset-password?type=parent&key=" + randomString + "\"\n                                            style=\"background:#2d2e2d;text-decoration:none !important; font-weight:500; margin-top:35px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\">\n                                            Şifremi Yenile\n                                        </a>\n                                    </td>\n                                </tr>\n                                <tr>\n                                    <td style=\"height:40px;\">&nbsp;</td>\n                                </tr>\n                            </table>\n                        </td>\n                    <tr>\n                        <td style=\"height:20px;\">&nbsp;</td>\n                    </tr>\n                    <tr>\n                        <td style=\"text-align:center;\">\n                            <p\n                                style=\"font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;\">\n                                &copy; <strong>www.aoflots.com</strong></p>\n                        </td>\n                    </tr>\n                    <tr>\n                        <td style=\"height:80px;\">&nbsp;</td>\n                    </tr>\n                </table>\n            </td>\n        </tr>\n    </table>\n    <!--/100% body table-->\n</body>\n\n</html>";
        var user = _parentService.Where(x => x.Email == resetPasswordLinkDto.mail).FirstOrDefault();
        if (user == null)
        {
            return new ErrorResult("Bu e posta hesabı bir kullanıcıya ait değil");
        }

        user.ActiveForgotLink = randomString;

        await _parentService.UpdateAsync(user);

        var mail = new MailServiceOptions()
        {
            SenderEmail = "info@aoflots.com",
            ReceiverEmail = resetPasswordLinkDto.mail,
            ReceiverName = user.Name,
            Body = mailBody,
            Subject = "Öğretmen Kayıt Kodu",
            SenderName = "Akçakoca Orhan Özdemir Fen Lisesi Öğrenci Takip Sistemi",
            SmtpPort = 465,
            SmtpServer = "smtpout.secureserver.net",
            SenderPassword = "l&3yikx257",
        };

        _mailService.SendEmail(mail);
        return new SuccessResult();
    }



    private static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }







}