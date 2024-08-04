using GTBack.Core.DTO;
using GTBack.Core.DTO.Ecommerce;
using GTBack.Core.DTO.Ecommerce.Request;
using GTBack.Core.DTO.Shopping.Filter;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Services.Ecommerce;
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
    
    
    [Microsoft.AspNetCore.Mvc.HttpPost("AddBasket")]
    public async Task<IActionResult> AddBasket(int variantId,string guid)
    {
        return ApiResult(await _productService.AddBasket(variantId,guid));
    }
    
    [Microsoft.AspNetCore.Mvc.HttpPost("GetBasket")]
    public async Task<IActionResult> GetBasket(string guid)
    {
        return ApiResult(await _productService.GetBasket(guid));
    }
}