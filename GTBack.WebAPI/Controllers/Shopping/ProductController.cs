using System.Web.Http;
using System.Xml.Serialization;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.DTO.Shopping;
using GTBack.Core.DTO.Shopping.Filter;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.DTO.Shopping.Response;
using GTBack.Core.Entities.Shopping;
using GTBack.Core.Results;
using GTBack.Core.Services.Shopping;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace GTBack.WebAPI.Controllers.Shopping;

public class ProductController : CustomShoppingBaseController
{
    
    private readonly IProductService _productService;
    private readonly IShoppingUserService _userService;

    
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [Microsoft.AspNetCore.Mvc.HttpPost("AddOrUpdateProduct")]
    public async Task<IActionResult> AddOrUpdateProduct(ProductAddDTO model)
    {
            
        return ApiResult(await _productService.AddOrUpdateProduct(model));
    }
    
    [Microsoft.AspNetCore.Mvc.HttpPost("RemoveProducts")]
    public async Task<IActionResult> RemoveProducts(List<long> model)
    {
            
        return ApiResult(await _productService.RemoveProducts(model));
    }
    
    [Microsoft.AspNetCore.Mvc.HttpGet("TarzYeri")]
    public async Task<IActionResult> TarzYeri()
    {   

        
        RecurringJob.AddOrUpdate(
            "listProduct",
            () =>  _productService.ParseJob(),
            "0 */3 * * *"
            );
       
        return ApiResult(new SuccessResult());
        
    }
    [Authorize]
    [Microsoft.AspNetCore.Mvc.HttpPost("FavoriteAddOrUpdate")]
    public async Task<IActionResult> FavoriteAddOrUpdate(string favorite)
    {

        _userService.AddFavoirte(favorite);
        return ApiResult(new SuccessResult());
        
    }

    
    [Microsoft.AspNetCore.Mvc.HttpGet("Fetch")]
    public async Task<IActionResult> Fetch()
    {
        
        
    
        return ApiResult(await _productService.ParseJob());
        
    }



           
    [Microsoft.AspNetCore.Mvc.HttpGet("TarzYeriList")]
    public async Task<IActionResult> TarzYeriList([FromQuery]BpmFilter filter)
    {
        return ApiResult(await _productService.GetTarzYeri(filter));
    }
            
    [Microsoft.AspNetCore.Mvc.HttpPost("ProductList")]
    public async Task<IActionResult> List(BaseListFilterDTO<ProductFilter> log)
    {
        return ApiResult(await _productService.GetProducts(log));
    }


}