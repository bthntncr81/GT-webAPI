using System.Security.Claims;
using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.DTO.Shopping.Response;
using GTBack.Core.Entities.Shopping;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Shopping;
using GTBack.Service.Utilities.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using XAct;

namespace GTBack.Service.Services.ShoppingService;

public class ShoppinOrderService:IShoppingOrderService
{
    private readonly IService<ShoppingOrder> _service;
    private readonly IService<Address> _addressService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ClaimsPrincipal? _loggedUser;
    private readonly IMapper _mapper;
    private readonly  IJwtTokenService<BaseRegisterDTO> _tokenService;
    private readonly IMemoryCache _cache;

    public ShoppinOrderService( IService<Address> addressService,IMemoryCache cache,IRefreshTokenService refreshTokenService,  IJwtTokenService<BaseRegisterDTO> tokenService,
        IHttpContextAccessor httpContextAccessor, IService<ShoppingOrder> service,
        IMapper mapper)
    {
        _mapper = mapper;
        _cache = cache;
        _service = service;
        _addressService = addressService;
        _loggedUser = httpContextAccessor.HttpContext?.User;
        _refreshTokenService = refreshTokenService;
        _tokenService = tokenService;
        
    }
    
    
    public async Task<IDataResults<OrderConfirmDTO>> CreateOrder(OrderConfirmDTO model)
    {
        Guid g = Guid.NewGuid();
        model.OrderGuid = g.ToString();
        if (!model.AddressId.IsNull())
        {
            var address = _mapper.Map<Address>(model.Address);  
          var addressElement= await  _addressService.AddAsync(address);
          model.AddressId = addressElement.Id;
        }
        var data = _mapper.Map<ShoppingOrder>(model);  
        await  _service.AddAsync(data);
        
        return new SuccessDataResult<OrderConfirmDTO>(model);
    }
    
    
    public async Task<IDataResults<ICollection<ShoppingOrderListDTO>>> GetOrdersByUserId(int id)
    {
     
       var orderRepo=   _service.Where(x=>x.ShoppingUserId==id);
       var addressRepo=   _addressService.Where(x=>!x.IsDeleted);

       var query = from order in orderRepo
           join address in addressRepo on order.AddressId equals address.Id into addressLeft
           from address in addressLeft.DefaultIfEmpty()
           select new ShoppingOrderListDTO()
           {        
               BasketJsonDetail = order.BasketJsonDetail,
               ShoppingUserId = order.ShoppingUserId.IsNull() ? order.ShoppingUserId:null,
               OrderGuid = order.OrderGuid,
               TotalPrice = order.TotalPrice,
               OrderNote = order.OrderNote,
               Status = order.Status,
               Name = order.Name,
               Surname = order.Surname,
               OrderDate = order.OrderDate,
               Phone = order.Phone,
               Mail = order.Mail,
               Address = new AddressResponseDTO
               {
                   Name = address.Name,
                   City = address.City,
                   District = address.District,
                   OpenAddress = address.OpenAddress,
               }
               
           };
       


       var orderList = await query.ToListAsync();



        
        return new SuccessDataResult<ICollection<ShoppingOrderListDTO>>(orderList);
    }

}