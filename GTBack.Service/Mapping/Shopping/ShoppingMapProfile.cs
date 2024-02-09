using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.DTO.Shopping.Response;
using GTBack.Core.Entities.Restourant;
using GTBack.Core.Entities.Shopping;

namespace GTBack.Service.Mapping.Resourant;

public class ShoppingMapProfile:Profile
{
    
        public ShoppingMapProfile()
        {
            //PRODUCT
            CreateMap<ProductListDTO, Product>().ReverseMap();
            CreateMap<ICollection<ProductListDTO>, IQueryable<Product>>().ReverseMap();
            CreateMap<Product, ProductAddDTO>().ReverseMap();
            
            
            CreateMap<ImageAddDTO, Image>().ReverseMap();
            // CreateMap<List<ImageAddDTO>, IList<Image>>().ReverseMap();
            
            
            CreateMap<Address, AddressAddDTO>().ReverseMap();
            CreateMap<ShoppingOrder, OrderConfirmDTO>().ReverseMap();

        }
    
}