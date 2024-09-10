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
    [Route("api/coach/auth")]
    public class CoachAuthController : CoachBaseController
    {
        private readonly ICoachAuthService _coachAuthService;
        private readonly IMapper _mapper;

        public CoachAuthController(ICoachAuthService coachAuthService, IMapper mapper)
        {
            _coachAuthService = coachAuthService;
            _mapper = mapper;
        }

        // [Authorize]
        // [HttpGet("me")]
        // public async Task<IActionResult> Me()
        // {
        //     return ApiResult(await _coachAuthService.GetById(_coachAuthService.GetLoggedUserId()));
        // }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            return ApiResult(await _coachAuthService.Login(loginDto));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CoachRegisterDTO registerDto)
        {
            return ApiResult(await _coachAuthService.Register(registerDto));
        }

        [HttpPost("update-account")]
        public async Task<IActionResult> UpdateAccount(CoachUpdateDTO updateDto)
        {
            return ApiResult(await _coachAuthService.UpdateCoach(updateDto));
        }

        [HttpPost("reset-password-link")]
        public async Task<IActionResult> SendResetPasswordLink([FromBody] ResetPasswordLinkDTO resetPasswordLinkDto)
        {
            return ApiResult(await _coachAuthService.ResetPasswordLink(resetPasswordLinkDto));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
        {
            return ApiResult(await _coachAuthService.ResetPassword(resetPasswordDto));
        }

        [HttpPost("delete-account")]
        public async Task<IActionResult> Delete(long id)
        {
            return ApiResult(await _coachAuthService.Delete(id));
        }
    }
}