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
using Hangfire;
using LinqToDB;
using Microsoft.EntityFrameworkCore;

namespace GTBack.WebAPI.Controllers.Shopping
{
    public class AuthController : CustomShoppingBaseController
    {
        private readonly IMapper _mapper;
        private readonly IService<ShoppingUser> _service;
        private readonly IService<XmlFiles> _xmlService;
        private readonly IShoppingUserService _userService;
        private readonly IShoppingCompany _company;

        public AuthController(IService<XmlFiles> xmlService,IShoppingCompany company,IService<ShoppingUser> service, IMapper mapper,IShoppingUserService userService)
        {
            _userService = userService;
            _service = service;
            _mapper = mapper;
            _company = company;
            _xmlService = xmlService;
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
            DateTime startTime = DateTime.Now;

           
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://cdn1.xmlbankasi.com/p1/bpmticaret/image/data/xml/Boabutik.xml");
            var request = new HttpRequestMessage(HttpMethod.Get, "");
            var response = await httpClient.SendAsync(request);
            DateTime endTime = DateTime.Now;
            TimeSpan elapsed = endTime - startTime;
            Console.WriteLine("Execution time: bpm" + elapsed.TotalMilliseconds + "ms");
            var json = response.Content.ReadAsStringAsync().Result;             
            return ApiResult(
                new SuccessDataResult<List<ProductBPM.ElementBpm>>(
                        await _userService.XmlConverterBpm(json, filter)));
          



        }

        
      
        [HttpGet("TarzYeri")]
        public async Task<IActionResult> TarzYeri([FromQuery] BpmFilter filter)
        {   
            DateTime startTime = DateTime.Now;

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://www.tarzyeri.com/export/ea6554eec9c42fa9dee93dbcbb7ee4d49UzdFk0LbWJOoD0Q==");
            var request = new HttpRequestMessage(HttpMethod.Get, "");
            var response = await httpClient.SendAsync(request);
            DateTime endTime = DateTime.Now;
            TimeSpan elapsed = endTime - startTime;
            Console.WriteLine("Execution time: " + elapsed.TotalMilliseconds + "ms");
            var json = response.Content.ReadAsStringAsync().Result;
            return ApiResult(new SuccessDataResult<List<ProductTarzYeri>>(await _userService.XmlConverter(json, filter)));



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