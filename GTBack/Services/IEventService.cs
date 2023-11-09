using GTBack.Core.DTO;
using GTBack.Core.Results;

namespace GTBack.Core.Services;

public interface IEventService
{ 
  
    Task<IDataResults<ICollection<EventListClientResponseDto>>> ListEventsByUserId(DateTime date);
    Task<IDataResults<ICollection<EventListClientResponseDto>>> ListAllEventsByUserId();
    Task<IDataResults<ICollection<EventListClientResponseDto>>> ListEventsByUserIdByDay(DateTime date);
    Task<IDataResults<ICollection<EventToMonthDTO>>> GetListDayByClientId(DateTime date);
    Task<IResults> ChangeEventTime(ChageEventTimeDto eventTime);
    Task<IResults>  CreateEvent(EventAddRequestDTO eventId);
    Task<IResults>  UpdateEvent(UpdateEventDTO updateEvent);
    Task<IDataResults<EventByEventId>> EventDetailByEventId(int eventId);
    Task<IResults> DeleteEvent(int eventId);
    Task<IResults> ChangeStatus(int statusId, int eventId);
    Task<IResults> CreateCompany(CreateCompanyDTO model);


}