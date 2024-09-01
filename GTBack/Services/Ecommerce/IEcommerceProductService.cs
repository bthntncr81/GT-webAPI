using GTBack.Core.DTO;
using GTBack.Core.DTO.Ecommerce;
using GTBack.Core.DTO.Ecommerce.Request;
using GTBack.Core.DTO.Ecommerce.Response;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Results;

namespace GTBack.Core.Services.Ecommerce;

public interface IEcommerceProductService
{
    Task<IResults> AddOrUpdateProduct(EcommerceProductAddDto model);

    Task<SuccessDataResult<List<Category>>> GetCategories(int id);

    Task<IDataResults<BaseListDTO<EcommerceProductListDTO, EcommerceProductListFilterRepresent>>> GetProducts(
        BaseListFilterDTO<EcommerceProductFilter> model);

    Task<IDataResults<BasketADDResponseDTO>> AddBasket(int variantId, string guid, long? clientId);
    Task<IDataResults<List<BasketDTO>>> GetBasket(string guid);


    Task<IDataResults<List<BasketDTO>>> GetBasketLogged();
    Task<IResults> RemoveBasket(int variantId, string guid, long? clientId);
    Task<IResults> UpdateVariant(EcommerceVariantUpdateDTO model);

    Task<IResults> RemoveSingleVariant(long id);

}