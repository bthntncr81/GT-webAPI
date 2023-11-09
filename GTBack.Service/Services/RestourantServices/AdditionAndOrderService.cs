using System.Security.Claims;
using AutoMapper;
using FirebaseAdmin.Messaging;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Entities.Restourant;
using GTBack.Core.Enums.Restourant;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Restourant;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using XAct;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Hangfire;

namespace GTBack.Service.Services.RestourantServices;

public class AdditionAndOrderService : IAdditionAndOrderService
{
    private readonly IService<Addition> _additionService;
    private readonly IService<Order> _orderService;
    private readonly IService<Table> _tableService;
    private readonly IService<OrderProcess> _orderProcessService;
    private readonly IService<ExtraMenuItem> _extraMenuService;
    private readonly IService<Employee> _employeeService;
    private readonly IService<MenuItem> _menuItemService;
    private readonly IService<TableArea> _tableAreaService;
    private readonly ITableAndAreaService _tableAndAreaService;
    public readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IMapper _mapper;
    private readonly ClaimsPrincipal? _loggedUser;


    public AdditionAndOrderService(
        IHttpContextAccessor httpContextAccessor,
        IService<OrderProcess> orderProcessService,
        IService<Table> tableService,
        IService<TableArea> tableAreaService,
        ITableAndAreaService tableAndAreaService,
        IService<Addition> additionService,
        IService<Order> orderService,
        IMapper mapper,
        IService<MenuItem> menuItemService,
        IService<ExtraMenuItem> extraMenuService,
        IService<Employee> employeeService,
        IBackgroundJobClient backgroundJobClient)
    {
        _mapper = mapper;
        _orderService = orderService;
        _employeeService = employeeService;
        _backgroundJobClient = backgroundJobClient;
        _tableAndAreaService = tableAndAreaService;
        _menuItemService = menuItemService;
        _extraMenuService = extraMenuService;
        _additionService = additionService;
        _orderProcessService = orderProcessService;
        _tableService = tableService;
        _tableAreaService = tableAreaService;
        _loggedUser = httpContextAccessor.HttpContext?.User;
    }

    public async Task<IResults> AdditionAddOrUpdate(AdditionAddOrUpdateDTO model)
    {
        model.IsClosed = false;
        if (model.Id != 0)
        {
            var response = _mapper.Map<Addition>(model);
            await _additionService.AddAsync(response);
            return new SuccessResult();
        }
        else
        {
            var response = _mapper.Map<Addition>(model);
            await _additionService.UpdateAsync(response);
            return new SuccessResult();
        }
    }

    public  void SendOrderNotification(string title, string body, string token, long orderId)
    {
        var orderProcessList =  _orderProcessService.Where(x => x.OrderId == orderId)
            .OrderByDescending(x => x.ChangeDate).ToList();

        if (orderProcessList[0].FinishedOrderStatus != OrderStatus.DELİVERED ||
            orderProcessList[0].FinishedOrderStatus != OrderStatus.CANCELED)
        {
            var message = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                    { "myData", "1337" },
                },
                Token = token,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                }
            };
            string response = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;
            Console.WriteLine("Successfully sent message: " + response);
        }
    }

    public async Task<IResults> OrderAddOrUpdate(OrderAddOrUpdateDTO model)
    {
        var extra = await _extraMenuService.Where(x => x.Id == model.ExtraMenuItemId).FirstOrDefaultAsync();
        var employee = await _employeeService.Where(x => x.Id == model.EmployeeId).FirstOrDefaultAsync();
        if (model.Id == 0)
        {
            var response = _mapper.Map<Order>(model);
            var addedOrder = await _orderService.AddAsync(response);

            var orderProcess = new OrderProcess()
            {
                InitialOrderStatus = OrderStatus.ORDERED,
                FinishedOrderStatus = OrderStatus.ORDERED,
                ChangeDate = DateTime.UtcNow,
                ChangeNote = addedOrder.OrderNote,
                OrderId = addedOrder.Id,
                EmployeeId = GetLoggedUserId()
            };

            await _orderProcessService.AddAsync(orderProcess);


            _backgroundJobClient.Schedule(() => SendOrderNotification("Teslimat Alarmı",
                    "Yiyeceğin tahmini teslim süresine son 10 dakika", employee.ApiKey,addedOrder.Id)
                , TimeSpan.FromMinutes(extra.EstimatedTime - 10));

            return new SuccessResult();
        }
        else
        {
            var response = _mapper.Map<Order>(model);
            await _orderService.UpdateAsync(response);
            return new SuccessResult();
        }
    }

    public async Task<IResults> OrderDelete(long id)
    {
        var order = await _orderService.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (order != null)
        {
            order.IsDeleted = true;
            await _orderService.UpdateAsync(order);
            return new SuccessResult();
        }
        else
        {
            return new ErrorResult("Order Not Found");
        }
    }

    public async Task<IResults> AdditionDelete(long id)
    {
        var addition = await _additionService.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (addition != null)
        {
            addition.IsDeleted = true;
            await _additionService.UpdateAsync(addition);
            return new SuccessResult();
        }
        else
        {
            return new ErrorResult("Addition Not Found");
        }
    }

    public async Task<IDataResults<BaseListDTO<AdditionListDTO, AdditionFilterRepresent>>> AllAdditionList(
        BaseListFilterDTO<AdditionFilterDTO> filter)
    {
        var companyId = GetLoggedCompanyId();
        var tableRepo = _tableService.Where(x => !x.IsDeleted);
        var tableAreaRepo = _tableAreaService.Where(x => !x.IsDeleted && x.RestoCompanyId == companyId);
        var additionRepo = _additionService.Where(x => !x.IsDeleted);


        var query = from addition in additionRepo
            join table in tableRepo on addition.TableId equals table.Id into tableLeft
            from table in tableLeft.DefaultIfEmpty()
            join tableArea in tableAreaRepo on table.TableAreaId equals tableArea.Id into tableAreaLeft
            from tableArea in tableAreaLeft.DefaultIfEmpty()
            select new AdditionListDTO()
            {
                Id = addition.Id,
                Name = addition.Name,
                TableNumber = table.TableNumber,
                TableId = table.Id,
                ClientId = addition.ClientId,
                IsClosed = addition.IsClosed,
                ClosedDate = addition.ClosedDate,
                TableAreaId = tableArea.Id,
                TableAreaName = tableArea.Name,
                CreatedDate = addition.CreatedDate
            };

        var myLis = await query.ToListAsync();
        if (filter.RequestFilter.IsClosed.HasValue)
        {
            query = query.Where(x => x.IsClosed == filter.RequestFilter.IsClosed);
        }

        if (!CollectionUtilities.IsNullOrEmpty(filter.RequestFilter.Name))
        {
            query = query.Where(x => x.Name.Contains(filter.RequestFilter.Name));
        }

        if (filter.RequestFilter.ClientId.HasValue)
        {
            query = query.Where(x => x.ClientId == filter.RequestFilter.ClientId);
        }

        if (!ObjectExtensions.IsNull(filter.RequestFilter.ClosedDate))
        {
            query = query.Where(x =>
                x.ClosedDate > filter.RequestFilter.ClosedDate.StartDate &&
                x.ClosedDate < filter.RequestFilter.ClosedDate.EndDate);
        }

        if (!ObjectExtensions.IsNull(filter.RequestFilter.CreatedDate))
        {
            query = query.Where(x =>
                x.CreatedDate > filter.RequestFilter.CreatedDate.StartDate &&
                x.CreatedDate < filter.RequestFilter.CreatedDate.EndDate);
        }


        if (filter.RequestFilter.TableNumber.HasValue)
        {
            query = query.Where(x => x.TableNumber == filter.RequestFilter.TableNumber);
        }

        if (!CollectionUtilities.IsNullOrEmpty(filter.RequestFilter.TableAreaName))
        {
            query = query.Where(x => x.TableAreaName.Contains(filter.RequestFilter.TableAreaName));
        }

        query = query.Skip(filter.PaginationFilter.Skip).Take(filter.PaginationFilter.Take);

        BaseListDTO<AdditionListDTO, AdditionFilterRepresent> additionList =
            new BaseListDTO<AdditionListDTO, AdditionFilterRepresent>();
        var response = _mapper.Map<ICollection<AdditionListDTO>>(await query.ToListAsync());
        additionList.List = response;
        additionList.Filter = new AdditionFilterRepresent();
        return new SuccessDataResult<BaseListDTO<AdditionListDTO, AdditionFilterRepresent>>(additionList);
    }

    public async Task<IDataResults<BaseListDTO<OrderListDTO, OrderFilterRepresent>>> OrderListByAdditionId(
        BaseListFilterDTO<OrderFilterDTO> filter, long additionId)
    {
        var query = _orderService.Where(x => x.AdditionId == additionId);

        var myObject = await query.ToListAsync();


        if (!CollectionUtilities.IsNullOrEmpty(filter.RequestFilter.Name))
        {
            query = query.Where(x => x.Name.Contains(filter.RequestFilter.Name));
        }


        if (!CollectionUtilities.IsNullOrEmpty(filter.RequestFilter.OrderNote))
        {
            query = query.Where(x => x.Name.Contains(filter.RequestFilter.OrderNote));
        }

        if (filter.RequestFilter.OrderStatus.HasValue)
        {
            query = query.Where(x => x.OrderStatus == filter.RequestFilter.OrderStatus);
        }

        if (filter.RequestFilter.AdditionId.HasValue)
        {
            query = query.Where(x => x.AdditionId == filter.RequestFilter.AdditionId);
        }


        if (!ObjectExtensions.IsNull(filter.RequestFilter.OrderDeliveredDate))
        {
            query = query.Where(x =>
                x.OrderDeliveredDate > filter.RequestFilter.OrderDeliveredDate.StartDate &&
                x.OrderDeliveredDate < filter.RequestFilter.OrderDeliveredDate.EndDate);
        }

        if (!ObjectExtensions.IsNull(filter.RequestFilter.OrderStartDate))
        {
            query = query.Where(x =>
                x.OrderStartDate > filter.RequestFilter.OrderStartDate.StartDate &&
                x.OrderStartDate < filter.RequestFilter.OrderStartDate.EndDate);
        }


        if (filter.RequestFilter.EmployeeId.HasValue)
        {
            query = query.Where(x => x.EmployeeId == filter.RequestFilter.EmployeeId);
        }

        if (filter.RequestFilter.ExtraMenuItemId.HasValue)
        {
            query = query.Where(x => x.ExtraMenuItemId == filter.RequestFilter.ExtraMenuItemId);
        }

        query = query.Skip(filter.PaginationFilter.Skip).Take(filter.PaginationFilter.Take);

        BaseListDTO<OrderListDTO, OrderFilterRepresent> orderList =
            new BaseListDTO<OrderListDTO, OrderFilterRepresent>();

        var response = _mapper.Map<ICollection<OrderListDTO>>(await query.ToListAsync());

        orderList.List = response;
        orderList.Filter = new OrderFilterRepresent();

        return new SuccessDataResult<BaseListDTO<OrderListDTO, OrderFilterRepresent>>(orderList);
    }


    public async Task<IResults> ChangeOrderStatus(ChangeOrderStatusDTO model)
    {
        var order = await _orderService.Where(x => !x.IsDeleted && x.Id == model.OrderId).FirstOrDefaultAsync();
        if (order != null)
        {
            var orderProcess = new OrderProcess()
            {
                InitialOrderStatus = order.OrderStatus,
                FinishedOrderStatus = model.OrderStatus,
                ChangeDate = DateTime.UtcNow,
                ChangeNote = model.ChangeNote,
                OrderId = order.Id,
                EmployeeId = GetLoggedUserId()
            };

            order.OrderStatus = model.OrderStatus;

            if (model.OrderStatus == OrderStatus.DELİVERED)
            {
                order.OrderDeliveredDate = DateTime.UtcNow;
            }


            await _orderProcessService.AddAsync(orderProcess);
            await _orderService.UpdateAsync(order);
            return new SuccessResult();
        }
        else
        {
            return new ErrorResult("Order Not Found");
        }
    }

    public async Task<IResults> CloseAddition(long additionId)
    {
        var addition = await _additionService.Where(x => !x.IsDeleted && x.Id == additionId).FirstOrDefaultAsync();
        if (addition != null)
        {
            addition.IsClosed = true;
            addition.ClosedDate = DateTime.UtcNow;
            await _additionService.UpdateAsync(addition);
            return new SuccessResult();
        }
        else
        {
            return new ErrorResult("Addition Not Found");
        }
    }


    private long? GetLoggedCompanyId()
    {
        var userRoleString = _loggedUser.FindFirstValue("companyId");
        if (long.TryParse(userRoleString, out var userId))
        {
            return userId;
        }

        return null;
    }

    private long GetLoggedUserId()
    {
        var userRoleString = _loggedUser.FindFirstValue("Id");
        if (long.TryParse(userRoleString, out var userId))
        {
            return userId;
        }

        return 0;
    }
}