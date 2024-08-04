using System.Text;
using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Ecommerce;
using GTBack.Core.DTO.Ecommerce.Request;
using GTBack.Core.DTO.Ecommerce.Response;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.DTO.Shopping.Filter;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.DTO.Shopping.Response;
using GTBack.Core.Entities.Ecommerce;
using GTBack.Core.Enums;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Ecommerce;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using XAct;

namespace GTBack.Service.Services.Ecommerce;

public class EcommerceProductService : IEcommerceProductService
{
    private readonly IService<EcommerceProduct> _productService;
    private readonly IService<EcommerceImage> _imageService;
    private readonly IService<EcommerceVariant> _variantService;
    private readonly IService<EcommerceBasket> _basketService;
    private readonly IService<EcommerceBasketProductRelation> _basketProdRelService;
    private readonly IMapper _mapper;
    private readonly IBackgroundJobClient _backgroundJobClient;


    public EcommerceProductService(IService<EcommerceBasket> basketService,IService<EcommerceBasketProductRelation> basketProdRelService,
        IService<EcommerceProduct> productService,
        IService<EcommerceImage> imageService, IMapper mapper, IService<EcommerceVariant> variantService)
    {
        _productService = productService;
        _imageService = imageService;
        _variantService = variantService;
        _basketService = basketService;
        _basketProdRelService = basketProdRelService;
        _mapper = mapper;
    }

    public async Task<SuccessDataResult<List<Category>>> GetCategories(int id)
    {
        var productRepo = _productService.Where(x => !x.IsDeleted&&x.EcommerceCompanyId==id); 
        var categories = productRepo
            .GroupBy(p => p.Category1)
            .Select(g1 => new Category
            {
                CategoryName = g1.Key,
                Children = g1.GroupBy(p => p.Category2)
                    .Select(g2 => new Category
                    {               
                        CategoryName = g2.Key,
                          Children = g2.GroupBy(p => p.Category3)
                    .Select(g3 => new Category
                    {
                        CategoryName = g3.Key
                    }).OrderBy(c => c.CategoryName).ToList()
                        
                    }).OrderBy(c => c.CategoryName).ToList()
            }).ToList();
        
      
        
        return new SuccessDataResult<List<Category>>(categories);
    }

    public async Task<IResults> AddOrUpdateProduct(EcommerceProductAddDto model)
    {
        var product = new EcommerceProduct();
        if (!model.Id.IsNull()&&model.Id!=0)
        {
            product = await _productService.FindAsNoTrackingAsync(x => x.Id == model.Id);
            var deletedVariants = await _variantService.Where(x => x.EcommerceProductId == model.Id).ToListAsync();

            foreach (var item in deletedVariants)
            {
                var deletedImages = await _imageService.Where(x => x.EcommerceVariantId == item.Id).ToListAsync();

                foreach (var ıtemImage in deletedImages)
                {
                    await _imageService.RemoveAsync(ıtemImage);
                }

                await _variantService.RemoveAsync(item);
            }
        }

        var productItem = new EcommerceProduct()
        {
            Id = model.Id ?? 0,
            EcommerceEmployeeId = model.EmployeeId,
            EcommerceCompanyId = model.CompanyId,
            Category1 = model.Category1,
            Category2 = model.Category2,
            Category3 = model.Category3,
            Brand = model.Brand
        };

        if (!model.Id.IsNull()&&model.Id!=0)
        {
            await _productService.UpdateAsync(productItem);
        }
        else
        {
            product = await _productService.AddAsync(productItem);
        }

        List<EcommerceVariant> variants;

        foreach (var item in model.Variants)
        {
            var variant = new EcommerceVariant()
            {
                EcommerceProductId = product.Id,
                Stock = item.Stock,
                Price = item.Price,
                Name = item.Name,
                Description = item.Description,
                VariantIndicator=item.VariantIndicator
            };
            var addedVariant = await _variantService.AddAsync(variant);

            foreach (var imageItem in item.Images)
            {
                var image = new EcommerceImage()
                {
                    Data = imageItem,
                    EcommerceVariantId = addedVariant.Id
                };
                await _imageService.AddAsync(image);
            }
        }


        return new SuccessResult("PRODUCT_ADDED");
    }

    
    public async Task<IResults> RemoveProducts(IList<long> idArray)
    {
    
        foreach (var id in idArray)
        {
            await _productService.RemoveAsync(await _productService.Where(x => x.Id == id).FirstOrDefaultAsync());
    
        }
        
        return new SuccessResult("PRODUCT_REMOVED");
    }

    public async Task<IResults> AddBasket(int variantId,string guid )
    {

        var basket = await _basketService.Where(x => x.Guid == guid).FirstOrDefaultAsync();

        if (basket.IsNull())
        {
            var basketModel = new EcommerceBasket()
            {
                Guid = GenerateRandomString(10)
            };
           var myBasket= await _basketService.AddAsync(basketModel);

           var basketRelModel = new EcommerceBasketProductRelation()
           {
               EcommerceVariantId = variantId,
               EcommerceBasketId = myBasket.Id,
               Count = 0
           };
        await  _basketProdRelService.AddAsync(basketRelModel);

        }else
        {
           var sameCountBasket= await _basketProdRelService.Where(x => x.EcommerceVariantId == variantId && x.EcommerceBasketId == basket.Id).FirstOrDefaultAsync();

           if (sameCountBasket.IsNull())
           {
               var basketRelModel = new EcommerceBasketProductRelation()
               {
                   EcommerceVariantId = variantId,
                   EcommerceBasketId = basket.Id,
                   Count = 0
               };
             await  _basketProdRelService.AddAsync(basketRelModel);
           }
           else
           {
               var count = sameCountBasket.Count;
               sameCountBasket.Count =count + 1;

               _basketProdRelService.UpdateAsync(sameCountBasket);

           }
          
        }

        return new SuccessResult();
    }
    
     public async Task<IDataResults<List<BasketDTO>>> GetBasket(string guid )
    {

        var basket = await _basketService.Where(x => x.Guid == guid).FirstOrDefaultAsync();
        var basketRelRepo =  _basketProdRelService.Where(x => x.EcommerceBasketId == basket.Id);
        var variantRepo =  _variantService.Where(x => !x.IsDeleted);
        var imageRepo =  _imageService.Where(x => !x.IsDeleted);

        var query = from variant in variantRepo
            join basketRel in basketRelRepo on variant.Id equals basketRel.EcommerceVariantId into basketRelLeft
            from basketRel in basketRelLeft
            select new BasketDTO()

            {
              Variants =  new EcommerceVariantListDTO()
              {
                  Id = variant.Id,
                  Images =   imageRepo.Where(x=>x.EcommerceVariantId==variant.Id).Select(x => x.Data).ToList(),
                  Price = variant.Price,
                  Name = variant.Name,
                  Stock = variant.Stock,
                  Description = variant.Description,
                  EcommerceProductId = variant.EcommerceProductId
              },
              Count = basketRel.Count
              
              
            };

        var myList = await query.ToListAsync();

        return new SuccessDataResult<List<BasketDTO>>(myList);
    }
    static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; // Define the character set
        Random random = new Random(); // Create a new instance of the Random class
        StringBuilder result = new StringBuilder(length);

        // Generate the random string
        for (int i = 0; i < length; i++)
        {
            // Get a random index in the range of the character set and append the character at that index to the result
            result.Append(chars[random.Next(chars.Length)]);
        }

        return result.ToString(); // Convert the StringBuilder to a string and return it
    }


    public async Task<IDataResults<BaseListDTO<EcommerceProductListDTO, EcommerceProductListFilterRepresent>>> GetProducts(
        BaseListFilterDTO<EcommerceProductFilter> model)
    {
        var productRepo = _productService.Where(x => !x.IsDeleted);
        var variantRepo = _variantService.Where(x => !x.IsDeleted);
        var imagesRepo = _imageService.Where(x => !x.IsDeleted);

     
        if (!ObjectExtensions.IsNull(model.RequestFilter.Name))
        {
            productRepo = productRepo.Where(p => p.Variants.Any(v => v.Name.Contains(model.RequestFilter.Name)));
        }
        if (!ObjectExtensions.IsNull(model.RequestFilter.Description))
        {
            productRepo = productRepo.Where(p => p.Variants.Any(v => v.Description.Contains(model.RequestFilter.Description)));
        }
        if (!ObjectExtensions.IsNull(model.RequestFilter.CompanyId))
        {
            productRepo = productRepo.Where(p => p.EcommerceCompanyId==model.RequestFilter.CompanyId);
        }
        
        if (!ObjectExtensions.IsNull(model.RequestFilter.Category1))
        {
            productRepo = productRepo.Where(x => x.Category1.ToLower().Contains(model.RequestFilter.Category1.ToLower()));
        }
         
        if (!ObjectExtensions.IsNull(model.RequestFilter.Category2))
        {
            productRepo = productRepo.Where(x => x.Category2.ToLower().Contains(model.RequestFilter.Category2.ToLower()));
        }
         
        if (!ObjectExtensions.IsNull(model.RequestFilter.Category3))
        {
            productRepo = productRepo.Where(x => x.Category3.ToLower().Contains(model.RequestFilter.Category3.ToLower()));
        }
        
    
        if (!ObjectExtensions.IsNull(model.RequestFilter.Price))
        {
            
     
            productRepo = productRepo.Where(p => p.Variants.Any(v => ((v.Price > model.RequestFilter.Price.Min)&&(v.Price < model.RequestFilter.Price.Max))));
        }
    
        if (!ObjectExtensions.IsNull(model.RequestFilter.Stock))
        {
            productRepo = productRepo.Where(p => p.Variants.Any(v => ((v.Stock > model.RequestFilter.Stock.Min)&&(v.Stock < model.RequestFilter.Stock.Max))));

        }
        
        if (!ObjectExtensions.IsNull(model.SortingFilter.ListOrderType))
        {
            if (model.SortingFilter.ListOrderType==ListOrderType.Ascending)
            {
                productRepo = productRepo.OrderBy(p => p.Variants.OrderBy(v => v.Id).FirstOrDefault());
            }
            else
            {
                productRepo = productRepo.OrderByDescending(p => p.Variants.OrderBy(v => v.Id).FirstOrDefault());
            }
        }
        
        
        var products = from product in productRepo
            select new EcommerceProductListDTO()
            {
                Id = product.Id,
                Category1 = product.Category1,
                Category2 = product.Category2,
                Category3 = product.Category3,
                Variants =  ( from variant in variantRepo.Where(x=>x.EcommerceProductId==product.Id)
                    select new EcommerceVariantListDTO()
                    {
                        Id = variant.Id,
                        Images =   imagesRepo.Where(x=>x.EcommerceVariantId==variant.Id).Select(x => x.Data).ToList(),
                        Price = variant.Price,
                        Name = variant.Name,
                        Stock = variant.Stock,
                        Description = variant.Description,
                        EcommerceProductId = variant.EcommerceProductId
                        
                          
                    }).ToList()                
            };
    
        BaseListDTO<EcommerceProductListDTO, EcommerceProductListFilterRepresent> productList =
            new BaseListDTO<EcommerceProductListDTO, EcommerceProductListFilterRepresent>();

       var listProduct = await products.ToListAsync();
  
    
        productList.List = _mapper.Map<ICollection<EcommerceProductListDTO>>(listProduct);
    
        productList.Filter = new EcommerceProductListFilterRepresent();
    
    
        return new SuccessDataResult<BaseListDTO<EcommerceProductListDTO, EcommerceProductListFilterRepresent>>(productList);
    }
}

