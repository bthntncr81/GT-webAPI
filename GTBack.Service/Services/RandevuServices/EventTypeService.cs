
using System.Net;
using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using GTBack.Core.DTO;
using GTBack.Core.Entities;
using GTBack.Core.Repositories;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.UnitOfWorks;
using GTBack.Service.Utilities.Jwt;
using GTBack.Service.Validation;
using GTBack.Service.Validation.Tool;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GTBack.Service.Services;

public class EventTypeService : IEventTypeService
{
    private readonly IGenericRepository<Event> _eventRepository;
    private readonly IGenericRepository<EventType> _eventTypeRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<EventTypeCompanyRelation> _eventTypeCompanyRelationRepository;
    private readonly IGenericRepository<Company> _companyRepository;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ClaimsPrincipal? _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidatorFactory _validatorFactory;
    private readonly IJwtTokenService<BaseRegisterDTO> _tokenService;

    public EventTypeService(IGenericRepository<Event> eventRepository,
        IGenericRepository<EventType> eventTypeRepository,
        IGenericRepository<User> userRepository,
        IGenericRepository<EventTypeCompanyRelation> eventTypeCompanyRelationRepository,
        IGenericRepository<Company> companyRepository,
        IRefreshTokenService refreshTokenService, IJwtTokenService<BaseRegisterDTO> tokenService,
        IValidatorFactory validatorFactory, IHttpContextAccessor httpContextAccessor, IService<Event> service,
        IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _eventRepository = eventRepository;
        _userRepository = userRepository;
        _eventTypeRepository = eventTypeRepository;
        _companyRepository = companyRepository;
        _eventTypeCompanyRelationRepository = eventTypeCompanyRelationRepository;
        _loggedUser = httpContextAccessor.HttpContext?.User;
        _validatorFactory = validatorFactory;
        _refreshTokenService = refreshTokenService;
        _tokenService = tokenService;
    }


    public async Task<IResults> CreateEventType(EventTypeAddRequestDTO model)
    {
        var eventType = new EventType()
        {
            Name = model.Name,
            Duration = model.Duration,
            Description = model.Description,
            IsDeleted = false,
            Price = model.Price,
            Id = 0

        };

       var createdEventType = await _eventTypeRepository.AddAsync(eventType);

       var eventTypeRel = new EventTypeCompanyRelation()
       {
           CompanyId = model.CompanyId,
           EventTypeId = createdEventType.Id,
           Id=0
       };
       
       await _eventTypeCompanyRelationRepository.AddAsync(eventTypeRel);


        return new SuccessResult();
    }



    public async Task<IDataResults<ICollection<EventTypeForDropdown>>> ListEventTypesForDropdownByCompanyId(int companyId)
    {
        
        
        var eventTypeRepo =  _eventTypeRepository.Where(x => !x.IsDeleted );
        var eventCompanyRelationRepo =  _eventTypeCompanyRelationRepository.Where(x => !x.IsDeleted&&x.CompanyId==companyId );
        var query = from eventType in eventTypeRepo
            join eventCompRepo in eventCompanyRelationRepo on eventType.Id equals eventCompRepo.EventTypeId into eventTypeCompUserLeft
            from eventCompRepo in eventTypeCompUserLeft.DefaultIfEmpty()
            select new EventTypeForDropdown()
            {
                Id = eventType.Id,
                Name = eventType.Name,
                Price = eventType.Price,
                Duration =eventType.Duration
            };
        
        var  eventTypeModel = _mapper.Map<ICollection<EventTypeForDropdown>>(await query.ToListAsync());
        
        return new SuccessDataResult<ICollection<EventTypeForDropdown>>(eventTypeModel, eventTypeModel.Count);
        
        

    }


 
    private int? GetLoggedUserId()
    {
        var userRoleString = _loggedUser.FindFirstValue("Id");
        if (int.TryParse(userRoleString, out var userId))
        {
            return userId;
        }

        return null;
    }
}