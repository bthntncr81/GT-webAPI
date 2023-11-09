using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Enums.Restourant;
using GTBack.Core.Results;

namespace GTBack.Core.Services.Restourant;

public interface IAdditionAndOrderService
{
    Task<IResults> AdditionAddOrUpdate(AdditionAddOrUpdateDTO model);
    Task<IResults> OrderAddOrUpdate(OrderAddOrUpdateDTO model);

    Task<IResults> OrderDelete(long id);
    Task<IResults> AdditionDelete(long id);

    Task<IDataResults<BaseListDTO<AdditionListDTO,AdditionFilterRepresent>>> AllAdditionList(BaseListFilterDTO<AdditionFilterDTO>  filter);
    Task<IDataResults<BaseListDTO<OrderListDTO,OrderFilterRepresent>>>OrderListByAdditionId(BaseListFilterDTO<OrderFilterDTO>  filter,long additionId);
    
    Task<IResults>  ChangeOrderStatus(ChangeOrderStatusDTO model);
    Task<IResults>  CloseAddition(long additionId);
}