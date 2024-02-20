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

    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProduct(ProductAddDTO model)
    {
            
        return ApiResult(await _productService.AddProduct(model));
    }
          
    [HttpGet("TarzYeri")]
    public async Task<IActionResult> TarzYeri()
    {   

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://www.tarzyeri.com/export/ea6554eec9c42fa9dee93dbcbb7ee4d49UzdFk0LbWJOoD0Q==");
        var request = new HttpRequestMessage(HttpMethod.Get, "");
        var response = await httpClient.SendAsync(request);
        var json = response.Content.ReadAsStringAsync().Result;
        
        using var httpClientBpm = new HttpClient();

        httpClientBpm.BaseAddress = new Uri("http://cdn1.xmlbankasi.com/p1/bpmticaret/image/data/xml/Boabutik.xml");
        var requestBpm = new HttpRequestMessage(HttpMethod.Get, "");
        var responseBpm = await httpClientBpm.SendAsync(requestBpm);
        var jsonBpm = responseBpm.Content.ReadAsStringAsync().Result;    
        
        
        
        XmlSerializer serializer = new XmlSerializer(typeof(ProductsTarzYeri));
        StringReader reader = new StringReader(json);
        ProductsTarzYeri myObject = (ProductsTarzYeri)serializer.Deserialize(reader);
        
        
        XmlSerializer serializerBpm = new XmlSerializer(typeof(ProductBPM.ProductBpms));
        StringReader readerBpm = new StringReader(jsonBpm);
        ProductBPM.ProductBpms myObjectBpm = (ProductBPM.ProductBpms)serializerBpm.Deserialize(readerBpm);

        
        RecurringJob.AddOrUpdate(
            "TarzYeri Kayıt",
            () =>  _productService.Job(myObject,myObjectBpm),
            Cron.MinuteInterval(10));
       
        

        return ApiResult(new SuccessResult());
        
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