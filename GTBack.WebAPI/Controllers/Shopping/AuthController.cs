using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Entities.Restourant;
using GTBack.Core.Entities.Shopping;
using GTBack.Core.Services;
using GTBack.Core.Services.Restourant;
using GTBack.Core.Services.Shopping;
using GTBack.Service.Services;
using GTBack.Service.Services.RestourantServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GTBack.WebAPI.Controllers.Shopping
{
    public class AuthController : CustomShoppingBaseController
    {
        private readonly IMapper _mapper;
        private readonly IService<ShoppingUser> _service;
        private readonly IShoppingUserService _userService;
        private readonly IShoppingCompany _company;

        public AuthController(IShoppingCompany company,IService<ShoppingUser> service, IMapper mapper,IShoppingUserService userService)
        {
            _userService = userService;
            _service = service;
            _mapper = mapper;
            _company = company;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            
            return ApiResult(await _userService.Me());
        }
            
            
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto log)
        {
            return ApiResult(await _userService.Login(log));
        }
            
        [HttpPost("RegisterCompany")]
        public async Task<IActionResult> RegisterCompany(ShoppingCompanyAddDTO log)
        {
            return ApiResult(await _company.AddShoppingCompany(log));
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(ClientRegisterRequestDTO request)
        {
            return ApiResult(await _userService.Register(request));
        }

        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginDTO request)
        {
            
            
            return ApiResult(await _userService.GoogleLogin(request));
        }


            

    }
}