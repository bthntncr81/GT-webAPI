


using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.Entities;
using GTBack.Core.Services;
using GTBack.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GTBack.WebAPI.Controllers
{
    public class EventTypeController : CustomRandevuBaseController
    {
      
      

        private readonly IMapper _mapper;
        private readonly IService<EventType> _service;
        private readonly IEventTypeService _eventTypeService;

        public EventTypeController(IService<EventType> service, IMapper mapper,IEventTypeService eventTypeService)
        {
            _eventTypeService = eventTypeService;
            _service = service;
            _mapper = mapper;
        }

        // [Authorize]
        [HttpPost("CreateEventType")]
        public async Task<IActionResult> CreateEventType(EventTypeAddRequestDTO eventTypeModel)
        {
            return ApiResult(await _eventTypeService.CreateEventType(eventTypeModel));
        }
        
        
        [HttpGet("ListEventTypesForDropdownByCompanyId")]
        public async Task<IActionResult> ListEventTypesForDropdownByCompanyId([FromQuery]int companyId)
        {
            return ApiResult(await _eventTypeService.ListEventTypesForDropdownByCompanyId(companyId));
        }
            
     



    }
}

