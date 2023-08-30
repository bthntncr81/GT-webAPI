using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GTBack.WebAPI.Controllers;

    public class EventController : CustomBaseController
    {
      
      

        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public EventController( IMapper mapper,IEventService eventService)
        {
            _eventService = eventService;
            _mapper = mapper;
        }
        
        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateEvent(EventAddRequestDTO model)
        {
            
            return ApiResult(await _eventService.createEvent(model));
            
        }
        [Authorize]
        [HttpGet("ListByClientId")]
        public async Task<IActionResult> CreateEvent()
        {
            return ApiResult(await _eventService.GetListByClientId());
        }
        

    }