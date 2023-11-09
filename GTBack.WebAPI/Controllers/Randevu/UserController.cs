

using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.Entities;
using GTBack.Core.Services;
using GTBack.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GTBack.WebAPI.Controllers
{
    public class UserController : CustomRandevuBaseController
    {
      
      

        private readonly IMapper _mapper;
        private readonly IService<User> _service;
        private readonly IUserService _userService;

        public UserController(IService<User> service, IMapper mapper,IUserService userService)
        {
            _userService = userService;
            _service = service;
            _mapper = mapper;
        }

        // [Authorize]
        [HttpGet("AdminListByCompanyId")]
        public async Task<IActionResult> Login(int companyId)
        {
            return ApiResult(await _userService.AdminListByCompanyId(companyId));
        }
            
     



    }
}

