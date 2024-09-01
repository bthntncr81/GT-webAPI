using GTBack.Core.DTO;
using GTBack.Core.DTO.Ecommerce;
using GTBack.Core.DTO.Ecommerce.Request;
using GTBack.Core.DTO.Shopping.Filter;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Services.Ecommerce;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GTBack.WebAPI.Controllers.Ecommerce;

public class EcommerceProductController : CustomEcommerceBaseController
{
    private readonly IEcommerceProductService _productService;

    
    public EcommerceProductController(IEcommerceProductService productService)
    {
        _productService = productService;
    }

    [Microsoft.AspNetCore.Mvc.HttpPost("AddOrUpdateProduct")]
    public async Task<IActionResult> AddOrUpdateProduct(EcommerceProductAddDto model)
    {
            
        return ApiResult(await _productService.AddOrUpdateProduct(model));
    }
    
    [Microsoft.AspNetCore.Mvc.HttpPost("UpdateVariant")]
    public async Task<IActionResult> UpdateVariant(EcommerceVariantUpdateDTO model)
    {
            
        return ApiResult(await _productService.UpdateVariant(model));
    }
    

    
    [Microsoft.AspNetCore.Mvc.HttpPost("ProductList")]
    public async Task<IActionResult> List(BaseListFilterDTO<EcommerceProductFilter> log)
    {
        return ApiResult(await _productService.GetProducts(log));
    }
    
    [Microsoft.AspNetCore.Mvc.HttpGet("GetCategories")]
    public async Task<IActionResult> GetCategories(int id)
    {
        return ApiResult(await _productService.GetCategories(id));
    }
    
    
    [Microsoft.AspNetCore.Mvc.HttpGet("AddBasket")]
    public async Task<IActionResult> AddBasket(int variantId,string guid,long? clientId)
    {
        return ApiResult(await _productService.AddBasket(variantId,guid,clientId));
    }
    
    [Microsoft.AspNetCore.Mvc.HttpGet("GetBasket")]
    public async Task<IActionResult> GetBasket(string guid)
    {
        return ApiResult(await _productService.GetBasket(guid));
    }
    
      
    [Microsoft.AspNetCore.Mvc.HttpGet("RemoveBasket")]
    public async Task<IActionResult> RemoveBasket(int variantId,string guid,long? clientId)
    {
        return ApiResult(await _productService.RemoveBasket(variantId,guid,clientId));
    }
    
    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpDelete("RemoveVariantById/{id}")]
    public async Task<IActionResult> RemoveSingleVariant(long id )
    {
        return ApiResult(await _productService.RemoveSingleVariant(id));
    }
    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpGet("GetBasketClient")]
    public async Task<IActionResult> GetBasketClient()
    {
        return ApiResult(await _productService.GetBasketLogged());
    }
}