using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.Entities;
using GTBack.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBack.Service.Mapping
{
    public class MapProfile:Profile
    {

        public MapProfile()
        {
            
          
            CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();
            CreateMap<UserRegisterDTO, User>().ReverseMap();
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<UserForDropdownDTO, User>().ReverseMap();
            CreateMap<Event, EventAddRequestDTO>().ReverseMap();
            CreateMap<ICollection<EventListClientResponseDto>, Event>().ReverseMap();
            CreateMap<EventListClientResponseDto, Event>().ReverseMap();
            CreateMap<EventToMonthDTO, Event>().ReverseMap();
            CreateMap<ICollection<EventListClientResponseDto>, IQueryable<Event>>().ReverseMap();
            CreateMap<ICollection<EventListClientResponseDto>, IQueryable<EventListClientResponseDto>>().ReverseMap();
            CreateMap<ICollection<EventToMonthDTO>, IQueryable<Event>>().ReverseMap();
            CreateMap<ICollection<Event>, IQueryable<EventToMonthDTO>>().ReverseMap();
            CreateMap<ICollection<UserForDropdownDTO>, IQueryable<User>>().ReverseMap();
            CreateMap<ICollection<UserForDropdownDTO>, User>().ReverseMap();
            CreateMap<ICollection<User>, IQueryable<User>>().ReverseMap();
            CreateMap<ICollection<EventTypeForDropdown>, IQueryable<EventType>>().ReverseMap();
            CreateMap<ICollection<EventTypeForDropdown>, EventType>().ReverseMap();
            CreateMap<EventTypeForDropdown, EventType>().ReverseMap();
            CreateMap<ICollection<EventByEventId>, IQueryable<Event>>().ReverseMap();
            CreateMap<ICollection<EventByEventId>, Event>().ReverseMap();
            CreateMap<EventByEventId, Event>().ReverseMap();
            CreateMap<CreateCompanyDTO, Company>().ReverseMap();
            CreateMap<EventByEventId, IQueryable<Event>>().ReverseMap();

            

        }
    }
}
