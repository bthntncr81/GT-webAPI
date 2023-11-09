
using GTBack.Core.DTO;
using GTBack.Core.Results;

namespace GTBack.Core.Services;

public interface IEventTypeService
{ 
    Task<IDataResults<ICollection<EventTypeForDropdown>>> ListEventTypesForDropdownByCompanyId(int companyId);
    Task<IResults> CreateEventType(EventTypeAddRequestDTO model);


}