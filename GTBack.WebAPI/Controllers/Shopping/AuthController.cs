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
        
        [HttpPost("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount(ClientUpdateDTO log)
        {
            return ApiResult(await _userService.UpdateUser(log));
        }
            
        [HttpPost("ResetPasswordLink")]
        public async Task<IActionResult> Login([FromBody] ResetPasswordLinkDTO userMail)
        {
            return ApiResult(await _userService.ResetPasswordLink(userMail));
        }
        
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO password)
        {
            return ApiResult(await _userService.ResetPassword(password));
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

        
        
        [HttpGet("Navlungo")]
        public async Task<IActionResult> TakeNavlongoAccess()
        {
            var verifier = "cb2cd18e-4d8e-4af3-995a-1923ef5dae41"; //buraya kendi örnek verifier'ınızı girerek oluşan hash'i görebilirsiniz
            var verifierBytes = System.Text.Encoding.UTF8.GetBytes(verifier);
			
            var sha256 = System.Security.Cryptography.SHA256.Create();
            var sha256ComputedVerifierHashBytes = sha256.ComputeHash(verifierBytes);
			
            string verifierHashedBase64String = Convert.ToBase64String(sha256ComputedVerifierHashBytes);
            Console.WriteLine($"Verifier: {verifier}");
            Console.WriteLine($"Hashed Verifier: {verifierHashedBase64String}");
            var result = new SuccessDataResult<string>(verifierHashedBase64String);
            return ApiResult(result);
        }

     
            

    }
}