using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.DTO.Restourant.Response.List;
using GTBack.Core.DTO.Shopping.Filter;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.DTO.Shopping.Response;
using GTBack.Core.Entities.Shopping;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Shopping;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using XAct;

namespace GTBack.Service.Services.ShoppingService;

public class ProductService:IProductService
{
    
    private readonly IService<Product> _productService;
    private readonly IService<Image> _imageService;
    private readonly IMapper _mapper;


    public ProductService(IService<Product> productService,IService<Image> imageService,  IMapper mapper)
    {
        _productService = productService;
        _imageService = imageService;
        _mapper = mapper;

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