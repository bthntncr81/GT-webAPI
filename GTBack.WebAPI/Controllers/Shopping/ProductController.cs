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

    
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("AddOrUpdateProduct")]
    public async Task<IActionResult> AddOrUpdateProduct(ProductAddDTO model)
    {
            
        return ApiResult(await _productService.AddOrUpdateProduct(model));
    }
    
    [HttpPost("RemoveProducts")]
    public async Task<IActionResult> RemoveProducts(List<long> model)
    {
            
        return ApiResult(await _productService.RemoveProducts(model));
    }
    
    [HttpGet("TarzYeri")]
    public async Task<IActionResult> TarzYeri()
    {   

        
        RecurringJob.AddOrUpdate(
            "listProduct",
            () =>  _productService.ParseJob(),
            "0 */3 * * *"
            );
       
        return ApiResult(new SuccessResult());
        
    }
    
    [HttpGet("Fetch")]
    public async Task<IActionResult> Fetch()
    {
        
        
    
        return ApiResult(await _productService.ParseJob());
        
    }



           
    [HttpGet("TarzYeriList")]
    public async Task<IActionResult> TarzYeriList([FromQuery]BpmFilter filter)
    {
        return ApiResult(await _productService.GetTarzYeri(filter));
    }
            
    [HttpPost("ProductList")]
    public async Task<IActionResult> List(BaseListFilterDTO<ProductFilter> log)
    {
        return ApiResult(await _productService.GetProducts(log));
    }


}