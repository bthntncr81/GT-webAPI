using System.Net;
using System.Security.Claims;
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
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using XAct;

namespace GTBack.Service.Services.Ecommerce;

public class EcommerceProductService : IEcommerceProductService
{
    private readonly IService<EcommerceProduct> _productService;
    private readonly IService<EcommerceImage> _imageService;
    private readonly IService<EcommerceVariant> _variantService;
    private readonly IService<EcommerceCompany> _companyService;
    private readonly IService<EcommerceBasket> _basketService;
    private readonly IService<EcommerceBasketProductRelation> _basketProdRelService;
    private readonly IService<EcommerceClient> _clientService;
    private readonly IMapper _mapper;
    private readonly ClaimsPrincipal? _loggedUser;
    private readonly IBackgroundJobClient _backgroundJobClient;


    public EcommerceProductService(IService<EcommerceCompany> companyService, IService<EcommerceClient> clientService, IService<EcommerceBasket> basketService, IService<EcommerceBasketProductRelation> basketProdRelService,
        IService<EcommerceProduct> productService,
        IService<EcommerceImage> imageService, IMapper mapper, IHttpContextAccessor httpContextAccessor, IService<EcommerceVariant> variantService)
    {

        _loggedUser = httpContextAccessor.HttpContext?.User;

        _productService = productService;
        _companyService = companyService;
        _imageService = imageService;
        _variantService = variantService;
        _basketService = basketService;
        _basketProdRelService = basketProdRelService;
        _clientService = clientService;
        _mapper = mapper;
    }

    public async Task<SuccessDataResult<List<Category>>> GetCategories(int id)
    {
        var productRepo = _productService.Where(x => !x.IsDeleted && x.EcommerceCompanyId == id).ToList();
        var categories = productRepo
            .Where(p => p != null && p.Category1 != null) // Check if the product and Category1 are not null
            .GroupBy(p => p.Category1.ToLower().Trim())
            .Where(g1 => !string.IsNullOrEmpty(g1.Key))
            .Select(g1 => new Category
            {
                CategoryName = g1.Key,
                Children = g1
                    .Where(p => p.Category2 != null) // Check if Category2 is not null
                    .GroupBy(p => p.Category2.ToLower().Trim())
                    .Where(g2 => !string.IsNullOrEmpty(g2.Key))
                    .Select(g2 => new Category
                    {
                        CategoryName = g2.Key,
                        Children = g2
                            .Where(p => p.Category3 != null) // Check if Category3 is not null
                            .GroupBy(p => p.Category3.ToLower().Trim())
                            .Where(g3 => !string.IsNullOrEmpty(g3.Key))
                            .Select(g3 => new Category
                            {
                                CategoryName = g3.Key
                            })
                            .OrderBy(c => c.CategoryName)
                            .ToList()
                    })
                    .OrderBy(c => c.CategoryName)
                    .ToList()
            })
            .OrderBy(c => c.CategoryName)
            .ToList();


        return new SuccessDataResult<List<Category>>(categories);
    }

    public async Task<IResults> UpdateVariant(EcommerceVariantUpdateDTO model)
    {
        var variant = await _variantService.FindAsNoTrackingAsync(x => x.Id == model.Id);
        var images = _imageService.Where(x => !x.IsDeleted && x.EcommerceVariantId == model.Id).ToList();

        if (variant.IsNull())
        {
            return new ErrorResult("There is no variant ");
        }
        await _variantService.UpdateAsync(_mapper.Map<EcommerceVariant>(model));

        foreach (var imageItem in images)
        {
            await _imageService.RemoveAsync(imageItem);
        }

        foreach (var imageItem in model.Images)
        {
            var image = new EcommerceImage()
            {
                Data = imageItem,
                EcommerceVariantId = model.Id
            };
            await _imageService.UpdateAsync(image);
        }
        return new SuccessResult();
    }

    public async Task<IResults> AddOrUpdateProduct(EcommerceProductAddDto model)
    {
        var product = new EcommerceProduct();
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

        if (!model.Id.IsNull() && model.Id != 0)
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
                EcommerceProductId = (model.Id.HasValue && model.Id != 0) ? model.Id.GetValueOrDefault() : product.Id,
                Stock = item.Stock,
                Price = item.Price,
                Name = item.Name,
                Description = item.Description,
                VariantCode = item.VariantCode,
                VariantIndicator = item.VariantIndicator,
                VariantName = item.VariantName,
                ThumbImage = item.ThumbImage
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
        // }

        return new SuccessResult("PRODUCT_ADDED");
    }


    public async Task<IResults> RemoveListProducts(IList<long> idArray)
    {

        foreach (var id in idArray)
        {

            var product = await _productService.Where(x => x.Id == id).FirstOrDefaultAsync();
            product.IsDeleted = true;
            _productService.UpdateAsync(product);
        }

        return new SuccessResult("PRODUCT_REMOVED");
    }
    public async Task<IResults> RemoveSingleProducts(long id)
    {

        var product = await _productService.Where(x => x.Id == id).FirstOrDefaultAsync();
        product.IsDeleted = true;
        _productService.UpdateAsync(product);
        return new SuccessResult("PRODUCT_REMOVED");
    }

    public async Task<IResults> RemoveSingleVariant(long id)
    {

        var variant = await _variantService.Where(x => x.Id == id).FirstOrDefaultAsync();
        variant.IsDeleted = true;
        _variantService.UpdateAsync(variant);
        return new SuccessResult("Variant Removed");
    }

    public async Task<IResults> RemoveBasket(int variantId, string guid, long? clientId)
    {
        var basket = await _basketService.Where(x => x.Guid == guid).FirstOrDefaultAsync();
        var basketRel = await _basketProdRelService.Where(x => x.EcommerceBasketId == basket.Id && x.EcommerceVariantId == variantId).FirstOrDefaultAsync();

        if (basketRel.Count == 1)
        {
            await _basketProdRelService.RemoveAsync(basketRel);
        }
        else
        {
            var count = basketRel.Count;
            basketRel.Count = count - 1;
            await _basketProdRelService.UpdateAsync(basketRel);
        }

        return new SuccessResult();

    }
    public async Task<IDataResults<BasketADDResponseDTO>> AddBasket(int variantId, string guid, long? clientId,int itemCount)
    {

        var myClient = await _clientService.Where(x => !x.IsDeleted && x.Id == clientId).FirstOrDefaultAsync();
        var basket = new EcommerceBasket();
        var isBasketExist = false;
        if (!myClient.IsNull())
        {

            basket = await _basketService.Where(x => !x.IsDeleted && x.Guid == guid || x.Id == myClient.BasketId).FirstOrDefaultAsync();
            if (!basket.IsNull())
            {
                isBasketExist = true;

            }
            else
            {
                isBasketExist = false;

            }
        }
        else
        {
            basket = await _basketService.Where(x => !x.IsDeleted && x.Guid == guid).FirstOrDefaultAsync();
            if (!basket.IsNull())
            {
                isBasketExist = true;
            }
        }



        if (!isBasketExist)
        {
            var basketModel = new EcommerceBasket()
            {
                Guid = GenerateRandomString(10)
            };

            var myBasket = await _basketService.AddAsync(basketModel);


            if (!clientId.IsNull())
            {
                var client = await _clientService.Where(x => !x.IsDeleted && x.Id == clientId).FirstOrDefaultAsync();
                client.BasketId = myBasket.Id;

                await _clientService.UpdateAsync(client);

            }
            var basketRelModel = new EcommerceBasketProductRelation()
            {
                EcommerceVariantId = variantId,
                EcommerceBasketId = myBasket.Id,
                Count = itemCount
            };
            await _basketProdRelService.AddAsync(basketRelModel);

            var basketResponse = new BasketADDResponseDTO();
            basketResponse.Guid = basketModel.Guid;
            return new SuccessDataResult<BasketADDResponseDTO>(basketResponse);

        }
        else
        {
            var sameCountBasket = await _basketProdRelService.Where(x => !x.IsDeleted && x.EcommerceVariantId == variantId && x.EcommerceBasketId == basket.Id).FirstOrDefaultAsync();

            if (sameCountBasket.IsNull())
            {
                var basketRelModel = new EcommerceBasketProductRelation()
                {
                    EcommerceVariantId = variantId,
                    EcommerceBasketId = basket.Id,
                    Count = itemCount
                };
                await _basketProdRelService.AddAsync(basketRelModel);
            }
            else
            {
                var count = sameCountBasket.Count;
                sameCountBasket.Count = count + itemCount;

                _basketProdRelService.UpdateAsync(sameCountBasket);

            }

            if (!clientId.IsNull())
            {
                var client = await _clientService.Where(x => !x.IsDeleted && x.Id == clientId).FirstOrDefaultAsync();
                client.BasketId = basket.Id;

                await _clientService.UpdateAsync(client);
            }
            var basketResponse = new BasketADDResponseDTO();
            basketResponse.Guid = basket.Guid;
            return new SuccessDataResult<BasketADDResponseDTO>(basketResponse);
        }

    }

    public async Task<IDataResults<List<BasketDTO>>> GetBasket(string guid,long companyId)
    {

        var basket = await _basketService.Where(x => !x.IsDeleted && x.Guid == guid).FirstOrDefaultAsync();
        var compRepo = _companyService.Where(x => !x.IsDeleted);
        var basketRelRepo = _basketProdRelService.Where(x => !x.IsDeleted && x.EcommerceBasketId == basket.Id);
        var variantRepo = _variantService.Where(x => !x.IsDeleted);
        var imageRepo = _imageService.Where(x => !x.IsDeleted);
        var productRepo = _productService.Where(x => !x.IsDeleted&&x.EcommerceCompanyId==companyId);

        var query = from variant in variantRepo
                    join basketRel in basketRelRepo on variant.Id equals basketRel.EcommerceVariantId into basketRelLeft
                    from basketRel in basketRelLeft
                    join prod in productRepo on variant.EcommerceProductId equals prod.Id into productLeft
                    from prod in productLeft
          
                    select new BasketDTO()

                    {
                        Variants = new EcommerceVariantListDTO()
                        {
                            Id = variant.Id,
                            Images = imageRepo.Where(x => !x.IsDeleted && x.EcommerceVariantId == variant.Id).Select(x => x.Data).ToList(),
                            Price = variant.Price,
                            Name = variant.Name,
                            Stock = variant.Stock,
                            Description = variant.Description,
                            EcommerceProductId = variant.EcommerceProductId,
                            ThumbImage = variant.ThumbImage
                        },
                        Category1 = prod.Category1,
                        Category2 = prod.Category2,
                        Category3 = prod.Category3,
                        Count = basketRel.Count


                    };

        var myList = await query.ToListAsync();

        return new SuccessDataResult<List<BasketDTO>>(myList);
    }


    public async Task<IDataResults<List<BasketDTO>>> GetBasketLogged()
    {
        var Id = _loggedUser.FindFirstValue("Id");
        var client = await _clientService.Where(x => x.Id == Int32.Parse(Id)).FirstOrDefaultAsync();
        var basketRelRepo = _basketProdRelService.Where(x => x.EcommerceBasketId == client.BasketId);
        var variantRepo = _variantService.Where(x => !x.IsDeleted);
        var imageRepo = _imageService.Where(x => !x.IsDeleted);
        var productRepo = _productService.Where(x => !x.IsDeleted);

        var query = from variant in variantRepo
                    join basketRel in basketRelRepo on variant.Id equals basketRel.EcommerceVariantId into basketRelLeft
                    from basketRel in basketRelLeft
                    join prod in productRepo on variant.EcommerceProductId equals prod.Id into productLeft
                    from prod in productLeft
                    select new BasketDTO()

                    {
                        Variants = new EcommerceVariantListDTO()
                        {
                            Id = variant.Id,
                            Images = imageRepo.Where(x => !x.IsDeleted && x.EcommerceVariantId == variant.Id).Select(x => x.Data).ToList(),
                            Price = variant.Price,
                            Name = variant.Name,
                            Stock = variant.Stock,
                            Description = variant.Description,
                            EcommerceProductId = variant.EcommerceProductId,
                            ThumbImage = variant.ThumbImage
                        },
                        Category1 = prod.Category1,
                        Category2 = prod.Category2,
                        Category3 = prod.Category3,
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
            productRepo = productRepo.Where(p => p.Variants.Any(v => v.Name.ToLower().Contains(model.RequestFilter.Name.ToLower())));
        }

        if (model.RequestFilter.Id != 0)
        {
            productRepo = productRepo.Where(p => p.Id == model.RequestFilter.Id);
        }
        if (!ObjectExtensions.IsNull(model.RequestFilter.Description))
        {
            productRepo = productRepo.Where(p => p.Variants.Any(v => v.Description.ToLower().Contains(model.RequestFilter.Description.ToLower())));
        }
        if (!ObjectExtensions.IsNull(model.RequestFilter.CompanyId))
        {
            productRepo = productRepo.Where(p => p.EcommerceCompanyId == model.RequestFilter.CompanyId);
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


            productRepo = productRepo.Where(p => p.Variants.Any(v => ((v.Price > model.RequestFilter.Price.Min) && (v.Price < model.RequestFilter.Price.Max))));
        }

        if (!ObjectExtensions.IsNull(model.RequestFilter.Stock))
        {
            productRepo = productRepo.Where(p => p.Variants.Any(v => ((v.Stock > model.RequestFilter.Stock.Min) && (v.Stock < model.RequestFilter.Stock.Max))));

        }

        if (!ObjectExtensions.IsNull(model.SortingFilter.ListOrderType))
        {
            if (model.SortingFilter.ListOrderType == ListOrderType.Ascending)
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
                           Category1 = product.Category1.ToLower().Trim(),
                           Category2 = product.Category2.ToLower().Trim(),
                           Category3 = product.Category3.ToLower().Trim(),
                           Brand = product.Brand.ToLower().Trim(),
                           Variants = (from variant in variantRepo.Where(x => x.EcommerceProductId == product.Id)
                                       select new EcommerceVariantListDTO()
                                       {
                                           Id = variant.Id,
                                           Images = imagesRepo.Where(x => !x.IsDeleted && x.EcommerceVariantId == variant.Id).Select(x => x.Data).ToList(),
                                           Price = variant.Price,
                                           Name = variant.Name,
                                           Stock = variant.Stock,
                                           Description = variant.Description,
                                           EcommerceProductId = variant.EcommerceProductId,
                                           VariantIndicator = variant.VariantIndicator,
                                           VariantCode = variant.VariantCode,
                                           VariantName = variant.VariantName,
                                           ThumbImage = variant.ThumbImage,


                                       }).ToList()
                       };

        BaseListDTO<EcommerceProductListDTO, EcommerceProductListFilterRepresent> productList =
            new BaseListDTO<EcommerceProductListDTO, EcommerceProductListFilterRepresent>();

        var listProduct = await products.ToListAsync();


        productList.List = _mapper.Map<ICollection<EcommerceProductListDTO>>(listProduct);

        productList.Filter = new EcommerceProductListFilterRepresent();


        return new SuccessDataResult<BaseListDTO<EcommerceProductListDTO, EcommerceProductListFilterRepresent>>(productList);
    }





    public async Task<IDataResults<BaseListDTO<EcommerceProductListGroupedDTO, EcommerceProductListFilterRepresent>>> GetGroupedProducts(
    BaseListFilterDTO<EcommerceProductFilter> model)
    {
        var productRepo = _productService.Where(x => !x.IsDeleted);
        var variantRepo = _variantService.Where(x => !x.IsDeleted);
        var imagesRepo = _imageService.Where(x => !x.IsDeleted);


        if (!ObjectExtensions.IsNull(model.RequestFilter.Name))
        {
            productRepo = productRepo.Where(p => p.Variants.Any(v => v.Name.ToLower().Contains(model.RequestFilter.Name.ToLower())));
        }

        if (model.RequestFilter.Id != 0)
        {
            productRepo = productRepo.Where(p => p.Id == model.RequestFilter.Id);
        }
        if (!ObjectExtensions.IsNull(model.RequestFilter.Description))
        {
            productRepo = productRepo.Where(p => p.Variants.Any(v => v.Description.ToLower().Contains(model.RequestFilter.Description.ToLower())));
        }
        if (!ObjectExtensions.IsNull(model.RequestFilter.CompanyId))
        {
            productRepo = productRepo.Where(p => p.EcommerceCompanyId == model.RequestFilter.CompanyId);
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


            productRepo = productRepo.Where(p => p.Variants.Any(v => ((v.Price > model.RequestFilter.Price.Min) && (v.Price < model.RequestFilter.Price.Max))));
        }

        if (!ObjectExtensions.IsNull(model.RequestFilter.Stock))
        {
            productRepo = productRepo.Where(p => p.Variants.Any(v => ((v.Stock > model.RequestFilter.Stock.Min) && (v.Stock < model.RequestFilter.Stock.Max))));

        }

        if (!ObjectExtensions.IsNull(model.SortingFilter.ListOrderType))
        {
            if (model.SortingFilter.ListOrderType == ListOrderType.Ascending)
            {
                productRepo = productRepo.OrderBy(p => p.Variants.OrderBy(v => v.Id).FirstOrDefault());
            }
            else
            {
                productRepo = productRepo.OrderByDescending(p => p.Variants.OrderBy(v => v.Id).FirstOrDefault());
            }
        }


        var products = from product in productRepo
                       select new EcommerceProductListGroupedDTO()
                       {
                           Id = product.Id,
                           Category1 = product.Category1.ToLower().Trim(),
                           Category2 = product.Category2.ToLower().Trim(),
                           Category3 = product.Category3.ToLower().Trim(),
                           Brand = product.Brand.ToLower().Trim(),
                           Variants = (
                               from variant in variantRepo.Where(x => x.EcommerceProductId == product.Id)
                               group variant by variant.ThumbImage into variantGroup
                               select new EcommerceVariantGroupDTO()
                               {
                                   ThumbImage = variantGroup.Key,
                                   Variants = variantGroup.Select(variant => new EcommerceVariantListDTO()
                                   {
                                       Id = variant.Id,
                                       Images = imagesRepo.Where(x => !x.IsDeleted && x.EcommerceVariantId == variant.Id).Select(x => x.Data).ToList(),
                                       Price = variant.Price,
                                       Name = variant.Name,
                                       Stock = variant.Stock,
                                       Description = variant.Description,
                                       EcommerceProductId = variant.EcommerceProductId,
                                       VariantIndicator = variant.VariantIndicator,
                                       VariantName = variant.VariantName,
                                       ThumbImage = variant.ThumbImage,
                                   }).ToList()
                               }).ToList()
                       };

        BaseListDTO<EcommerceProductListGroupedDTO, EcommerceProductListFilterRepresent> productList =
            new BaseListDTO<EcommerceProductListGroupedDTO, EcommerceProductListFilterRepresent>();

        var listProduct = await products.ToListAsync();


        productList.List = _mapper.Map<ICollection<EcommerceProductListGroupedDTO>>(listProduct);

        productList.Filter = new EcommerceProductListFilterRepresent();


        return new SuccessDataResult<BaseListDTO<EcommerceProductListGroupedDTO, EcommerceProductListFilterRepresent>>(productList);
    }

}

