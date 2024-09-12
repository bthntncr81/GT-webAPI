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

    public StudentAuthService( IService<Coach> coachService,IMemoryCache cache, IRefreshTokenService refreshTokenService, IJwtTokenService<BaseRegisterDTO> tokenService,
        IHttpContextAccessor httpContextAccessor, IService<Student> studentService, IMapper mapper)
    {
        _mapper = mapper;
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

        var coach =await _coachService.Where(x => x.ActiveCoachGuid == registerDto.CoachGuid).FirstOrDefaultAsync();
        var student = new Student
        {
            Name = registerDto.Name,
            Surname = registerDto.Surname,
            Email = registerDto.Mail,
            Phone = registerDto.Phone,
            PasswordHash = SHA1.Generate(registerDto.Password),
            IsDeleted = false,
            Grade = registerDto.Grade,
            CoachId = coach.Id
        };

        await _studentService.AddAsync(student);

        coach.ActiveCoachGuid = null;

        await _coachService.UpdateAsync(coach);

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

        if (student == null ||SHA1.Verify(loginDto.Password, student.PasswordHash))
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

        var student = await _studentService.Where(x => x.CoachId ==Int32.Parse(userIdClaim) ).Select(s=>new StudentUpdateDTO()
        {
            Id=s.Id,
            Name = s.Name,
            Surname = s.Surname,
            Grade = s.Grade,
            Phone = s.Phone,
            Email = s.Email,
            
            
        }).ToListAsync();

        return new SuccessDataResult<List<StudentUpdateDTO>>(student);

    }

    // Send Reset Password Link
    public async Task<IResults> ResetPasswordLink(ResetPasswordLinkDTO resetPasswordLinkDto)
    {
        var email = resetPasswordLinkDto.mail.ToLower().Trim();
        var student = await _studentService.Where(x => x.Email == email && !x.IsDeleted).FirstOrDefaultAsync();

        if (student == null)
        {
            return new ErrorResult("Student with this email does not exist", HttpStatusCode.NotFound);
        }

        string resetLink = GenerateRandomString(40); // Random string for reset link
        student.ActiveForgotLink = resetLink;
        await _studentService.UpdateAsync(student);

        // Prepare reset email
        string mailBody = $"Please click on the link to reset your password: <a href='https://yourwebsite.com/reset-password?key={resetLink}'>Reset Password</a>";

        var mailData = new MailData
        {
            SenderMail = "your-email@domain.com",
            RecieverMail = resetPasswordLinkDto.mail,
            EmailSubject = "Reset Your Password",
            EmailBody = mailBody
        };

        SendMail(mailData); // You should implement SendMail method for sending the email

        return new SuccessResult("Reset password link sent successfully", HttpStatusCode.OK);
    }

    // Authenticate Student (generates tokens)
    private async Task<AuthenticatedUserResponseDto> Authenticate(StudentRegisterDTO studentDto)
    {
        var accessToken = _tokenService.GenerateAccessToken(studentDto,true);
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