using System.Text.Json;
using System.Xml.Serialization;
using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.DTO.Restourant.Response.List;
using GTBack.Core.DTO.Shopping;
using GTBack.Core.DTO.Shopping.Filter;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.DTO.Shopping.Response;
using GTBack.Core.Entities.Shopping;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Shopping;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using XAct;

namespace GTBack.Service.Services.ShoppingService;

public class ProductService : IProductService
{

    private readonly IService<Product> _productService;
    private readonly IService<Image> _imageService;
    private readonly IService<GlobalProductModel> _globalProductService;
    private readonly IService<LastUpdated> _lastUpdatedService;
    private readonly IService<MyVariant> _variantService;
    private readonly IMapper _mapper;
    private readonly IBackgroundJobClient _backgroundJobClient;



    public ProductService(IService<LastUpdated> lastUpdatedService, IService<MyVariant> variantService,
        IService<GlobalProductModel> globalProductService, IService<Product> productService,
        IService<Image> imageService, IMapper mapper)
    {
        _productService = productService;
        _imageService = imageService;
        _mapper = mapper;
        _variantService = variantService;
        _globalProductService = globalProductService;
        _lastUpdatedService = lastUpdatedService;

    }

    public async Task<IResults> ParseJob()
    {

        using var httpClient = new HttpClient();
        httpClient.BaseAddress =
            new Uri("https://www.tarzyeri.com/export/ea6554eec9c42fa9dee93dbcbb7ee4d49UzdFk0LbWJOoD0Q==");
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

       await Job(myObject, myObjectBpm);
        return new SuccessResult();

    }


public async Task<IResults> Job(ProductsTarzYeri myObject,  ProductBPM.ProductBpms bpmObject)
    {       
        

        
        foreach (var item in bpmObject.ProductList)
            {
                var element = new GlobalProductModel()
                {
                    ProductId = !item.Product_id.IsNullOrEmpty()? item.Product_id:null,
                    ProductCode = !item.Product_code.IsNullOrEmpty()?item.Product_code:null,
                    MainCategory = !item.MainCategory.IsNullOrEmpty()? item.MainCategory:null,
                    Barcode = "null",
                    SubCategory = !item.SubCategory.IsNullOrEmpty()?item.SubCategory:null,
                    Category = !item.Category.IsNullOrEmpty()?item.Category:null,
                    Brand = !item.Brand.IsNullOrEmpty()?item.Brand:null,
                    Name = !item.Name.IsNullOrEmpty()?item.Name:null,
                    Description = !item.Description.IsNullOrEmpty()?item.Description:null,
                    NotDiscountedPrice =  !item.Price.IsNullOrEmpty()?item.Price:null,
                    Price = !item.Price2.IsNullOrEmpty()?item.Price2:null,
                    Detail = null,
                    Quantity = !item.Stock.IsNullOrEmpty()?item.Stock:null,
                };
                var imageString = "";
                if (!item.Image1.IsNullOrEmpty())
                {
                    imageString= imageString +"/clipper/image/"+item.Image1.Replace( " ", "" );
                }
                if (!item.Image2.IsNullOrEmpty())
                {
                    imageString= imageString +"/clipper/image/"+item.Image2.Replace( " ", "" );
        
                }
                if (!item.Image3.IsNullOrEmpty())
                {
                    imageString= imageString +"/clipper/image/"+item.Image3.Replace( " ", "" );
        
                }
                if (!item.Image4.IsNullOrEmpty())
                {
                    imageString= imageString +"/clipper/image/"+item.Image4.Replace( " ", "" );
        
                }
                if (!item.Image5.IsNullOrEmpty())
                {
                    imageString= imageString +"/clipper/image/"+item.Image5.Replace( " ", "" );
        
                }
        
        
                element.Images = imageString;
                var variantString = "";
                if (!item.Variants.IsNull())
                {
                    foreach (var variant in item.Variants.Variant)
                    {

                        foreach (var elem in variant.Specs)
                        {
                            if (Int32.Parse(variant.Quantity) != 0 && (elem.Name == "Beden"||elem.Name=="Numara"))
                            {
                                var variantModel = new MyVariant()
                                {

                                    Size = elem.Value,
                                    VariantId = variant.VariantId,
                                    Quantity = variant.Quantity,
                                };
                                variantString = variantString + "/clipper/variant/" + JsonSerializer.Serialize(variantModel);

                            }
                        }

                    }

                    
                }
                
            
                element.Variants = !variantString.IsNull() ? variantString : "empty";
                var updatedElement=_globalProductService.Where(x => x.ProductId == element.ProductId).AsNoTracking().FirstOrDefault();
                var globalProduct = new GlobalProductModel();
                if (!updatedElement.IsNull())
                {
                   
                    if (element.ProductId == "19702")
                    {
                        Console.WriteLine();
                    }
                         element.Id=updatedElement.Id;
                    await   _globalProductService.UpdateAsync(element);
                     

                       
                         
                }
                else
                {
                     globalProduct= await _globalProductService.AddAsync(element);
        
                }
                
            }
        
        foreach (var item in myObject.ProductList)
        {
            
                var element = new GlobalProductModel()
                {
                    ProductId = !item.id.IsNullOrEmpty()? item.id:null,
                    ProductCode = !item.productCode.IsNullOrEmpty()?item.productCode:null,
                    Barcode = !item.barcode.IsNullOrEmpty()?item.barcode:null,
                    MainCategory = !item.main_category.IsNullOrEmpty()? item.main_category:null,
                    SubCategory = !item.sub_category.IsNullOrEmpty()?item.sub_category:null,
                    Category = !item.category.IsNullOrEmpty()?item.category:null,
                    BrandId = !item.brandID.IsNullOrEmpty()?item.brandID:null,
                    Brand = !item.brand.IsNullOrEmpty()?item.brand:null,
                    Name = !item.name.IsNullOrEmpty()?item.name:null,
                    Description = !item.description.IsNullOrEmpty()?item.description:null,
                    NotDiscountedPrice = null,
                    Price = !item.price.IsNullOrEmpty()?item.price:null,
                    Detail = !item.detail.IsNullOrEmpty()?item.detail:null,
                    Quantity = !item.quantity.IsNullOrEmpty()?item.quantity:null,
                };
                var imageString = "";
                if (!item.image1.IsNullOrEmpty())
                {
                    imageString= imageString +"/clipper/image/"+item.image1.Replace( " ", "" );
                }
                if (!item.image2.IsNullOrEmpty())
                {
                    imageString= imageString +"/clipper/image/"+item.image2.Replace( " ", "" );
        
                }
                if (!item.image3.IsNullOrEmpty())
                {
                    imageString= imageString +"/clipper/image/"+item.image3.Replace( " ", "" );
        
                }
                if (!item.image4.IsNullOrEmpty())
                {
                    imageString= imageString +"/clipper/image/"+item.image4.Replace( " ", "" );
        
                }
                if (!item.image5.IsNullOrEmpty())
                {
                    imageString= imageString +"/clipper/image/"+item.image5.Replace( " ", "" );
        
                }

                Console.WriteLine(element);

                element.Images = imageString;
               
              

                var myVariantString = "";
                
                foreach (var variant in item.variants.VariantList)
                {
                    
                    var variantModel = new MyVariant()
                    {
                        Size = variant.value2,
                        VariantId = variant.barcode,
                        Quantity = variant.quantity,
                        
                    };
                    myVariantString = myVariantString + "/clipper/variant/" + JsonSerializer.Serialize(variantModel);
                }

                element.Variants = !myVariantString.IsNull() ? myVariantString : "empty";
                var updatedElement=_globalProductService.Where(x => x.ProductId == element.ProductId).FirstOrDefault();
                var globalProduct = new GlobalProductModel();

                if (!updatedElement.IsNull())
                {
                  
                        element.Id=updatedElement.Id;
                        _globalProductService.UpdateAsync(element);
                    
              
                }
                else
                {
                    globalProduct= await _globalProductService.AddAsync(element);

                }                
                
           
        }
        
        
        
        return new SuccessResult();
    }

    public async Task<IDataResults<List<GlobalProductModelResponseDTO>>> GetTarzYeri(BpmFilter filter)
    {

        var productRepo =  _globalProductService.Where(x => !x.IsDeleted);

        if (!filter.MainCategory.IsNullOrEmpty())
        {
            
            productRepo = productRepo.Where(x => x.MainCategory.ToUpper().Contains(filter.MainCategory.Replace('i','ı').ToUpper()));
        
        }

        if (!filter.SubCategory.IsNullOrEmpty())
        {
            productRepo = productRepo.Where(x => x.SubCategory.ToLower().Contains(filter.SubCategory)||x.Category.ToLower().Contains(filter.SubCategory));
        }
        
        
        if (!filter.Id.IsNullOrEmpty())
        {
            productRepo = productRepo.Where(x => x.ProductId==filter.Id);
        }

        
        

        var query = (from product in productRepo
            select new GlobalProductModelResponseDTO()
            {
                Id = product.Id,
                ProductId = !product.ProductId.IsNullOrEmpty() ? product.ProductId : null,
                ProductCode = !product.ProductCode.IsNullOrEmpty() ? product.ProductCode : null,
                Barcode = !product.Barcode.IsNullOrEmpty() ? product.Barcode : null,
                MainCategory = !product.MainCategory.IsNullOrEmpty() ? product.MainCategory : null,
                SubCategory = !product.SubCategory.IsNullOrEmpty() ? product.SubCategory : null,
                Category = !product.Category.IsNullOrEmpty() ? product.Category : null,
                BrandId = !product.BrandId.IsNullOrEmpty() ? product.BrandId : null,
                Brand = !product.Brand.IsNullOrEmpty() ? product.Brand : null,
                Name = !product.Name.IsNullOrEmpty() ? product.Name : null,
                Description = !product.Description.IsNullOrEmpty() ? product.Description : null,
                NotDiscountedPrice = !product.NotDiscountedPrice.IsNullOrEmpty() ? product.NotDiscountedPrice : null,
                Images =  !product.Images.IsNullOrEmpty() ? product.Images : null,
                Price = !product.Price.IsNullOrEmpty() ? product.Price : null,
                Detail = !product.Detail.IsNullOrEmpty() ? product.Detail : null,
                Quantity = !product.Quantity.IsNullOrEmpty() ? product.Quantity : null,
                Variants =  product.Variants
            });

    


      query= query.Skip(filter.Skip).Take(filter.Take);
       

        
        
        return new SuccessDataResult<List<GlobalProductModelResponseDTO>>(query.ToList());
    }
    

    public async Task<IResults> AddProduct(ProductAddDTO model)
    {
        var shoppingCompany = new Product()
        {
            Name = model.Name,
            Stock = model.Stock,
            Price = model.Price,
            CategoryId = model.CategoryId,
            CollectionId = model.CollectionId,
            ShoppingCompanyId= model.ShoppingCompanyId
        };
        
       var product= await _productService.AddAsync(shoppingCompany);

        foreach (var item in model.Image)
        {
            var imageItem = new Image()
            {
                ProductId = product.Id,
                Data = item.Data,

            };
           await _imageService.AddAsync(imageItem);
        }
        
        return new SuccessResult();
    }
    
    
    public async Task<IDataResults<BaseListDTO<ProductListDTO,ProductListFilterRepresent>>> GetProducts(BaseListFilterDTO<ProductFilter> model)
    {
        var query = _productService.Where(x => !x.IsDeleted);

        if (!ObjectExtensions.IsNull(model.RequestFilter.CategoryEnum))
        {
            foreach (var item in model.RequestFilter.CategoryEnum)
            {
                query = _productService.Where(x => x.CategoryId == item);
            }
        }
        
        if (!ObjectExtensions.IsNull(model.RequestFilter.CollectionEnum)) {
            foreach (var item in model.RequestFilter.CollectionEnum)
            {
                query = _productService.Where(x => x.CollectionId == item);
            }
        } 
        
        if (!CollectionUtilities.IsNullOrEmpty(model.RequestFilter.Name))
        {
            query = query.Where(x => x.Name.Contains(model.RequestFilter.Name));
        }

        
        if (!ObjectExtensions.IsNull(model.RequestFilter.Price))
        {
            query = query.Where(x =>
                x.Price < model.RequestFilter.Price.Max && x.Price > model.RequestFilter.Price.Min);
        }

        if (!ObjectExtensions.IsNull(model.RequestFilter.Stock))
        {
            query = query.Where(x =>
                x.Stock < model.RequestFilter.Stock.Max && x.Stock > model.RequestFilter.Stock.Min);
        }

        
        
        BaseListDTO<ProductListDTO, ProductListFilterRepresent> productList =
            new BaseListDTO<ProductListDTO, ProductListFilterRepresent>();
        
       var listProduct = _mapper.Map<ICollection<ProductListDTO>>(await query.ToListAsync());
        
       foreach (var item in listProduct)
       {
            

           var imageList = await _imageService.Where(x => x.ProductId == item.Id).ToListAsync();
        
        
            
           var addedImage = _mapper.Map<List<ImageAddDTO>>(imageList);
        
           item.Image = addedImage;
        
       }
       
        productList .List=_mapper.Map<ICollection<ProductListDTO>>(listProduct);
        
        productList.Filter = new ProductListFilterRepresent();

        
        return new SuccessDataResult<BaseListDTO<ProductListDTO, ProductListFilterRepresent>>(productList);

    }
}