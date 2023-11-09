using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.Entities.Restourant;
using GTBack.Core.Services;
using GTBack.Core.Services.Restourant;
using GTBack.Service.Services;
using GTBack.Service.Services.RestourantServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GTBack.WebAPI.Controllers.Restourant
{
    public class AuthController : CustomRestourantBaseController
    {
        
        private readonly IMapper _mapper;
     
        private readonly IClientService _clientService;
        private readonly IEmployeeService _employeeService;

        public AuthController( IMapper mapper,IClientService clientService,IEmployeeService employeeService)
        {
            _clientService = clientService;
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            
            return ApiResult(await _clientService.Me());
        }
            
            
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto log)
        {
            return ApiResult(await _clientService.Login(log));
        }
            
        [HttpPost("Register")]
        public async Task<IActionResult> Register(ClientRegisterRequestDTO request)
        {
            
            
            return ApiResult(await _clientService.Register(request));
        }
        [HttpPost("EmployeeRegister")]
        public async Task<IActionResult> EmpRegister(EmployeeRegisterDTO request)
        {
            return ApiResult(await _employeeService.Register(request));
        }
        [Authorize]
        [HttpPost("EmplooyeSelectPassowrd")]
        public async Task<IActionResult> EmplooyeSelectPassowrd(PasowordConfirmDTO request)
        {
            return ApiResult(await _employeeService.PasswordChoose(request));
        }
        
        [HttpPost("EmployeeLogin")]
        public async Task<IActionResult> EmployeeLogin(LoginRestourantDTO log)
        {
            return ApiResult(await _employeeService.Login(log));
        }

        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginDTO request)
        {
            return ApiResult(await _clientService.GoogleLogin(request));
        }


            

    }
}