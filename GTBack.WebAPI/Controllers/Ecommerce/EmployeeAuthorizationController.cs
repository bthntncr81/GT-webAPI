using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Entities.Shopping;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Shopping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GTBack.Core.DTO.Shopping;
using GTBack.Core.DTO.Shopping.Filter;
using GTBack.Core.Services.Ecommerce;
using CompanyAddDTO = GTBack.Core.DTO.Ecommerce.CompanyAddDTO;

namespace GTBack.WebAPI.Controllers.Ecommerce
{
    public class EmployeeAuthorizationController : CustomEcommerceBaseController
    {
        
        private readonly IEmployeeAuthService _authService;
        private readonly IEcommerceCompanyService _company;

        public EmployeeAuthorizationController(IEmployeeAuthService authService,IEcommerceCompanyService company)
        {
            _authService = authService;
            _company = company;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            return ApiResult(await _authService.Me());
        }
   
            
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto log)
        {
            return ApiResult(await _authService.Login(log));
        }
        
        [HttpPost("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount(ClientUpdateDTO log)
        {
            return ApiResult(await _authService.UpdateUser(log));
        }
            
        [HttpPost("ResetPasswordLink")]
        public async Task<IActionResult> Login([FromBody] ResetPasswordLinkDTO userMail)
        {
            return ApiResult(await _authService.ResetPasswordLink(userMail));
        }
        
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO password)
        {
            return ApiResult(await _authService.ResetPassword(password));
        }

        [HttpPost("RegisterCompany")]
        public async Task<IActionResult> RegisterCompany(CompanyAddDTO log)
        {
            return ApiResult(await _company.AddShoppingCompany(log));
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(ClientRegisterRequestDTO request)
        {
            return ApiResult(await _authService.Register(request));
        }

        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginDTO request)
        {
            
            return ApiResult(await _authService.GoogleLogin(request));
        }
        
    }
}