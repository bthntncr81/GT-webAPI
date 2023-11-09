using AutoMapper;
using GTBack.Core.DTO;

using GTBack.Core.Entities;
using GTBack.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GTBack.WebAPI.Controllers;

public class EventController : CustomRandevuBaseController
{



    private readonly IMapper _mapper;
    private readonly IEventService _eventService;
    private readonly IMailService _mailService;

    public EventController(IMapper mapper, IEventService eventService, IMailService mailService)
    {
        _eventService = eventService;
        _mapper = mapper;
        _mailService = mailService;
    }

    [Authorize]
    [HttpPost("Create")]

    public async Task<IActionResult> CreateEvent(EventAddRequestDTO model)
    {

        return ApiResult(await _eventService.CreateEvent(model));
    }

    [Authorize]
    [HttpPost("CreateCompany")]

    public async Task<IActionResult> CreateEvent(CreateCompanyDTO model)
    {

        return ApiResult(await _eventService.CreateCompany(model));
    }

    [Authorize]
    [HttpDelete("Delete")]

    public async Task<IActionResult> DeleteEvent(int Id)
    {

        return ApiResult(await _eventService.DeleteEvent(Id));
    }

    [Authorize]
    [HttpGet("ChangeStatus")]

    public async Task<IActionResult> UpdateStatusIdEvent(int statusId, int eventId)
    {

        return ApiResult(await _eventService.ChangeStatus(statusId, eventId));
    }
    [Authorize]
    [HttpGet("ListByClientId")]
    public async Task<IActionResult> GetListByClientId([FromQuery] DateTime date)
    {
        return ApiResult(await _eventService.GetListDayByClientId(date));
    }

    [Authorize]
    [HttpGet("ListEventsByUserId")]
    public async Task<IActionResult> ListEventsByUserId([FromQuery] DateTime date)
    {
        return ApiResult(await _eventService.ListEventsByUserId(date));
    }

    [Authorize]
    [HttpGet("ListAllEventsByUserId")]
    public async Task<IActionResult> ListEventsByUserId()
    {
        return ApiResult(await _eventService.ListAllEventsByUserId());
    }
    [Authorize]
    [HttpGet("ListEventsByUserIdByDay")]
    public async Task<IActionResult> ListEventsByUserIdByDay([FromQuery] DateTime date)
    {
        return ApiResult(await _eventService.ListEventsByUserIdByDay(date));
    }

    [Authorize]
    [HttpPost("ChangeEventTime")]
    public async Task<IActionResult> ChangeEventTime(ChageEventTimeDto eventTime)
    {
        return ApiResult(await _eventService.ChangeEventTime(eventTime));
    }

    [Authorize]
    [HttpPost("UpdateEvent")]
    public async Task<IActionResult> UpdateEvent(UpdateEventDTO updateEvent)
    {
        return ApiResult(await _eventService.UpdateEvent(updateEvent));
    }

    [Authorize]
    [HttpGet("EventDetailByEventId")]
    public async Task<IActionResult> EventDetailByEventId([FromQuery] int eventId)
    {
        return ApiResult(await _eventService.EventDetailByEventId(eventId));
    }


    [HttpPost("SendMail")]
    public Task SendMail(MailData data)
    {
        return _mailService.SendMail(data);
    }
}
