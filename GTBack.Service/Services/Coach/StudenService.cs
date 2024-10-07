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
using GTBack.Core.Services.Ecommerce;
using GTBack.Service.Utilities;
using GTBack.Service.Utilities.Jwt;
using GTBack.Service.Validation.Tool;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public class StudentAuthService : IStudentAuthService
{
    private readonly IService<Student> _studentService;
    private readonly IService<Coach> _coachService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ClaimsPrincipal? _loggedUser;
    private readonly IMapper _mapper;
    private readonly IJwtTokenService<BaseRegisterDTO> _tokenService;
    private readonly IMemoryCache _cache;

    private readonly IMailService _mailService;


    public StudentAuthService(IMailService mailService, IService<Coach> coachService, IMemoryCache cache, IRefreshTokenService refreshTokenService, IJwtTokenService<BaseRegisterDTO> tokenService,
        IHttpContextAccessor httpContextAccessor, IService<Student> studentService, IMapper mapper)
    {
        _mapper = mapper;
        _mailService = mailService;
        _cache = cache;
        _studentService = studentService;
        _coachService = coachService;
        _loggedUser = httpContextAccessor.HttpContext?.User;
        _refreshTokenService = refreshTokenService;
        _tokenService = tokenService;
    }

    // Get Student by ID
    public async Task<IDataResults<UserDTO>> GetById(long id)
    {
        var student = await _studentService.GetByIdAsync(x => x.Id == id);
        if (student == null)
        {
            return new ErrorDataResults<UserDTO>("Student not found", HttpStatusCode.NotFound);
        }

        var data = _mapper.Map<UserDTO>(student);
        return new SuccessDataResult<UserDTO>(data);
    }

    // Register Student
    public async Task<IDataResults<AuthenticatedUserResponseDto>> Register(StudentRegisterDTO registerDto)
    {
        var validationResult = FluentValidationTool.ValidateModelWithKeyResult(new StudentRegisterValidator(), registerDto);

        if (!validationResult.Success)
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>(HttpStatusCode.BadRequest, validationResult.Errors);
        }

        var email = registerDto.Mail.ToLower().Trim();
        var existingStudent = await _studentService.Where(x => x.Email.ToLower() == email && !x.IsDeleted).FirstOrDefaultAsync();

        if (existingStudent != null)
        {
            validationResult.Errors.Add("", "Email already exists");
            return new ErrorDataResults<AuthenticatedUserResponseDto>(HttpStatusCode.BadRequest, validationResult.Errors);
        }

        var coach = await _coachService.Where(x => x.ActiveCoachGuid == registerDto.CoachGuid).FirstOrDefaultAsync();
        var student = new Student
        {
            Name = registerDto.Name,
            Surname = registerDto.Surname,
            Email = registerDto.Mail,
            Phone = registerDto.Phone,
            PasswordHash = SHA1.Generate(registerDto.Password),
            IsDeleted = false,
            Grade = registerDto.Grade,
            CoachId = coach.Id,
            HavePermission = false
        };

        await _studentService.AddAsync(student);

        var response = await Authenticate(_mapper.Map<StudentRegisterDTO>(student));
        return new SuccessDataResult<AuthenticatedUserResponseDto>(response, HttpStatusCode.OK);
    }

    // Update Student
    public async Task<IDataResults<StudentUpdateDTO>> UpdateStudent(StudentUpdateDTO updateDto)
    {
        var student = await _studentService.GetByIdAsync(x => x.Id == updateDto.Id && !x.IsDeleted);
        if (student == null)
        {
            return new ErrorDataResults<StudentUpdateDTO>("Student not found", HttpStatusCode.NotFound);
        }

        student.Name = updateDto.Name;
        student.Surname = updateDto.Surname;
        student.Email = updateDto.Email;
        student.Phone = updateDto.Phone;
        student.Grade = updateDto.Grade;
        student.HavePermission = updateDto.HavePermission;

        await _studentService.UpdateAsync(student);
        return new SuccessDataResult<StudentUpdateDTO>(updateDto, HttpStatusCode.OK);
    }

    // Delete Student
    public async Task<IResults> Delete(long id)
    {
        var student = await _studentService.GetByIdAsync(x => x.Id == id);
        if (student == null)
        {
            return new ErrorResult("Student not found", HttpStatusCode.NotFound);
        }

        student.IsDeleted = true;
        await _studentService.UpdateAsync(student);
        return new SuccessResult("Student deleted successfully", HttpStatusCode.OK);
    }

    // Login
    public async Task<IDataResults<AuthenticatedUserResponseDto>> Login(LoginDto loginDto)
    {
        // var validationResult = FluentValidationTool.ValidateModelWithKeyResult(new StudentLoginValidator(), loginDto);
        // if (!validationResult.Success)
        // {
        //     return new ErrorDataResults<AuthenticatedUserResponseDto>(HttpStatusCode.BadRequest, validationResult.Errors);
        // }

        var email = loginDto.Mail.ToLower().Trim();
        var student = await _studentService.Where(x => x.Email.ToLower() == email && !x.IsDeleted).FirstOrDefaultAsync();

        if (student == null || !SHA1.Verify(loginDto.Password, student.PasswordHash))
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>("Invalid email or password", HttpStatusCode.BadRequest);
        }

        var response = await Authenticate(_mapper.Map<StudentRegisterDTO>(student));
        return new SuccessDataResult<AuthenticatedUserResponseDto>(response);
    }

    // Reset Password
    public async Task<IResults> ResetPassword(ResetPasswordDTO passwordDto)
    {
        var student = await _studentService.Where(x => x.ActiveForgotLink == passwordDto.ActiveLink).FirstOrDefaultAsync();
        if (student == null)
        {
            return new ErrorResult("Invalid or expired link", HttpStatusCode.BadRequest);
        }

        student.PasswordHash = SHA1.Generate(passwordDto.NewPassword);
        student.ActiveForgotLink = string.Empty;
        await _studentService.UpdateAsync(student);

        return new SuccessResult("Password reset successfully", HttpStatusCode.OK);
    }

    public async Task<IDataResults<List<StudentUpdateDTO>>> GetStudentsByCoachId()
    {

        var userIdClaim = _loggedUser.FindFirstValue("Id");

        var student = await _studentService.Where(x => x.CoachId == Int32.Parse(userIdClaim)).Select(s => new StudentUpdateDTO()
        {
            Id = s.Id,
            Name = s.Name,
            Surname = s.Surname,
            Grade = s.Grade,
            Phone = s.Phone,
            Email = s.Email,
            HavePermission = s.HavePermission,


        }).ToListAsync();

        return new SuccessDataResult<List<StudentUpdateDTO>>(student);

    }

    // Send Reset Password Link
    public async Task<IResults> ResetPasswordLink(ResetPasswordLinkDTO resetPasswordLinkDto)
    {
        string randomString = GenerateRandomString(40);
        string mailBody =
            "<!doctype html>\n<html lang=\"en-US\">\n\n<head>\n    <meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />\n    <title>Reset Password Email Template</title>\n    <meta name=\"description\" content=\"Reset Password Email Template.\">\n    <style type=\"text/css\">\n        a:hover {\n            text-decoration: underline !important;\n        }\n    </style>\n</head>\n\n<body marginheight=\"0\" topmargin=\"0\" marginwidth=\"0\" style=\"margin: 0px; background-color: #f2f3f8;\" leftmargin=\"0\">\n    <!--100% body table-->\n    <table cellspacing=\"0\" border=\"0\" cellpadding=\"0\" width=\"100%\" bgcolor=\"#f2f3f8\"\n        style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\">\n        <tr>\n            <td>\n                <table style=\"background-color: #f2f3f8; max-width:670px;  margin:0 auto;\" width=\"100%\" border=\"0\"\n                    align=\"center\" cellpadding=\"0\" cellspacing=\"0\">\n                    <tr>\n                        <td style=\"height:80px;\">&nbsp;</td>\n                    </tr>\n                    <tr>\n                        <td style=\"text-align:center;\">\n                            <a href=\"https://www.aoflots.com\" title=\"logo\" target=\"_blank\">\n                                <img width=\"150\" src=\"https://www.aoflots.com/assets/logo-blue.png\" title=\"logo\"\n                                    alt=\"logo\">\n                            </a>\n                        </td>\n                    </tr>\n                    <tr>\n                        <td style=\"height:20px;\">&nbsp;</td>\n                    </tr>\n                    <tr>\n                        <td>\n                            <table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"\n                                style=\"max-width:670px;background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);\">\n                                <tr>\n                                    <td style=\"height:40px;\">&nbsp;</td>\n                                </tr>\n                                <tr>\n                                    <td style=\"padding:0 35px;\">\n                                        <h1\n                                            style=\"color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;\">\n                                            Şifrenizi sıfırlama talebinde bulundunuz</h1>\n                                        <span\n                                            style=\"display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;\"></span>\n                                        <p style=\"color:#455056; font-size:15px;line-height:24px; margin:0;\">\n                                            Size eski şifrenizi öylece gönderemeyiz. Şifrenizi sıfırlamak için benzersiz\n                                            bir bağlantı sizin için oluşturuldu. Şifrenizi sıfırlamak için aşağıdaki\n                                            bağlantıya tıklayın ve talimatları izleyin.\n                                        </p>\n                                        <a href=\"https://www.aoflots.com/auth/reset-password?type=student&key=" + randomString + "\"\n                                            style=\"background:#2d2e2d;text-decoration:none !important; font-weight:500; margin-top:35px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\">\n                                            Şifremi Yenile\n                                        </a>\n                                    </td>\n                                </tr>\n                                <tr>\n                                    <td style=\"height:40px;\">&nbsp;</td>\n                                </tr>\n                            </table>\n                        </td>\n                    <tr>\n                        <td style=\"height:20px;\">&nbsp;</td>\n                    </tr>\n                    <tr>\n                        <td style=\"text-align:center;\">\n                            <p\n                                style=\"font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;\">\n                                &copy; <strong>www.aoflots.com</strong></p>\n                        </td>\n                    </tr>\n                    <tr>\n                        <td style=\"height:80px;\">&nbsp;</td>\n                    </tr>\n                </table>\n            </td>\n        </tr>\n    </table>\n    <!--/100% body table-->\n</body>\n\n</html>";
        var user = _studentService.Where(x => x.Email == resetPasswordLinkDto.mail).FirstOrDefault();
        if (user == null)
        {
            return new ErrorResult("Bu e posta hesabı bir kullanıcıya ait değil");
        }

        user.ActiveForgotLink = randomString;

        await _studentService.UpdateAsync(user);

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

    // Authenticate Student (generates tokens)
    private async Task<AuthenticatedUserResponseDto> Authenticate(StudentRegisterDTO studentDto)
    {
        var accessToken = _tokenService.GenerateAccessTokenStudent(studentDto,studentDto.HavePermission);
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

    // Helper method to generate a random string (for reset password link)
    private static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    // Send email (mocked or implemented)
    private void SendMail(MailData mailData)
    {
        var smtpClient = new SmtpClient("smtp-mail.outlook.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential("your-email@domain.com", "your-password")
        };

        var message = new MailMessage(mailData.SenderMail, mailData.RecieverMail, mailData.EmailSubject, mailData.EmailBody)
        {
            IsBodyHtml = true
        };

        smtpClient.Send(message);
    }


}