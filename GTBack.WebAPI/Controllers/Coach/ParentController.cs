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
    [Route("api/parent/auth")]
    public class ParentController : CoachBaseController
    {
        private readonly IParentAuthService _parentAuthService;
        private readonly IMapper _mapper;

        public ParentController(IParentAuthService parentAuthService, IMapper mapper)
        {
            _parentAuthService = parentAuthService;
            _mapper = mapper;
        }

 

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            return ApiResult(await _parentAuthService.Login(loginDto));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(ParentRegisterDTO registerDto)
        {
            return ApiResult(await _parentAuthService.Register(registerDto));
        }

        [HttpPost("update-account")]
        public async Task<IActionResult> UpdateAccount(CoachUpdateDTO updateDto)
        {
            return ApiResult(await _parentAuthService.UpdateCoach(updateDto));
        }

        [HttpPost("reset-password-link")]
        public async Task<IActionResult> SendResetPasswordLink([FromBody] ResetPasswordLinkDTO resetPasswordLinkDto)
        {
            return ApiResult(await _parentAuthService.ResetPasswordLink(resetPasswordLinkDto));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
        {
            return ApiResult(await _parentAuthService.ResetPassword(resetPasswordDto));
        }

        [HttpPost("delete-account")]
        public async Task<IActionResult> Delete(long id)
        {
            return ApiResult(await _parentAuthService.Delete(id));
        }
        
    

    }
}