using GTBack.Core.DTO.Ecommerce;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Results;

namespace GTBack.Core.Services.Shopping;

public interface IEcommerceCompanyService
{
    Task<IResults> AddShoppingCompany(CompanyAddDTO model);

    Task<IDataResults<CompanyAddDTO>> GetCompany(int id);

}