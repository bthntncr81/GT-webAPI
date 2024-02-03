using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Entities.Restourant;
using GTBack.Core.Entities.Shopping;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Restourant;
using GTBack.Core.Services.Shopping;
using GTBack.Service.Services;
using GTBack.Service.Services.RestourantServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;
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

        [HttpGet("BPM")]
        public async Task<IActionResult> BPM([FromQuery] BpmFilter filter)
        {
            using var httpClient = new HttpClient();

            httpClient.BaseAddress =
                new Uri("http://cdn1.xmlbankasi.com/p1/bpmticaret/image/data/xml/Boabutik.xml");
            var request = new HttpRequestMessage(HttpMethod.Get, "");
  
            var response = await httpClient.SendAsync(request);
            var json = response.Content.ReadAsStringAsync().Result;

            return ApiResult(new SuccessDataResult<List<ProductBPM.ElementBpm>>(_userService.XmlConverterBpm(json,filter)));

        }

        [HttpGet("TarzYeri")]
        public async Task<IActionResult> TarzYeri([FromQuery] BpmFilter filter)
        {
            using var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri("https://www.tarzyeri.com/export/ea6554eec9c42fa9dee93dbcbb7ee4d49UzdFk0LbWJOoD0Q==");
            var request = new HttpRequestMessage(HttpMethod.Get, "");
            request.Headers.Add("ApiKey","5f0c7e38d8a9c61b23770399fbcfadb4OHOmr3xKK8ByUauzyVbYcqWBVRywRSCa62A9UDkbsyxDHKYHAvxLbw==");
            // request.Headers.Add("Content-Type", "application/json");
            var response = await httpClient.SendAsync(request);
            var json = response.Content.ReadAsStringAsync().Result;
            
            return ApiResult(new SuccessDataResult<List<ProductTarzYeri>>(_userService.XmlConverter(json,filter)));
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