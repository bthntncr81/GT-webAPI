using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Entities.Restourant;
using GTBack.Core.Entities.Shopping;

namespace GTBack.Service.Mapping.Resourant;

public class RestourantMapProfile:Profile
{
    
        public RestourantMapProfile()
        {
            //CLİENT
   
            CreateMap<ShoppingUser, UserDTO>().ReverseMap();
          
        }
    
}