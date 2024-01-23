using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.DTO.Shopping.Filter;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.DTO.Shopping.Response;
using GTBack.Core.Results;

namespace GTBack.Core.Services.Shopping;

public interface IProductService
{
    Task<IDataResults<BaseListDTO<ProductListDTO, ProductListFilterRepresent>>> GetProducts(
        BaseListFilterDTO<ProductFilter> model);

    Task<IResults> AddProduct(ProductAddDTO model);
}