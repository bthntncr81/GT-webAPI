using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Results;

namespace GTBack.Core.Services.Shopping;

public interface IShoppingCompany
{
    Task<IResults> AddShoppingCompany(ShoppingCompanyAddDTO model);

}