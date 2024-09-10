using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GTBack.Core.DTO.Coach.Request;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Services.coach;

namespace GTBack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/student/auth")]
    public class StudentAuthController : CoachBaseController
    {
        private readonly IStudentAuthService _studentAuthService;
        private readonly IMapper _mapper;

        public StudentAuthController(IStudentAuthService studentAuthService, IMapper mapper)
        {
            _studentAuthService = studentAuthService;
            _mapper = mapper;
        }

        // [Authorize]
        // [HttpGet("me")]
        // public async Task<IActionResult> Me()
        // {
        //     return ApiResult(await _studentAuthService.GetById(_studentAuthService.GetLoggedUserId()));
        // }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            return ApiResult(await _studentAuthService.Login(loginDto));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(StudentRegisterDTO registerDto)
        {
            return ApiResult(await _studentAuthService.Register(registerDto));
        }

        [HttpPost("update-account")]
        public async Task<IActionResult> UpdateAccount(StudentUpdateDTO updateDto)
        {
            return ApiResult(await _studentAuthService.UpdateStudent(updateDto));
        }

        [HttpPost("reset-password-link")]
        public async Task<IActionResult> SendResetPasswordLink([FromBody] ResetPasswordLinkDTO resetPasswordLinkDto)
        {
            return ApiResult(await _studentAuthService.ResetPasswordLink(resetPasswordLinkDto));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
        {
            return ApiResult(await _studentAuthService.ResetPassword(resetPasswordDto));
        }

        [HttpPost("delete-account")]
        public async Task<IActionResult> Delete(long id)
        {
            return ApiResult(await _studentAuthService.Delete(id));
        }
    }
}