using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.Entities;
using GTBack.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTBack.Core.DTO.Coach.Request;
using GTBack.Core.DTO.Ecommerce.Request;
using GTBack.Core.DTO.Ecommerce.Response;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Entities.Coach;
using GTBack.Core.Entities.Ecommerce;
using GTBack.Core.Entities.Shopping;
using CompanyAddDTO = GTBack.Core.DTO.Ecommerce.CompanyAddDTO;

namespace GTBack.Service.Mapping
{
    public class MapProfile:Profile
    {

        public MapProfile()
        {
            
          
            CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();
            CreateMap<UserRegisterDTO, ShoppingUser>().ReverseMap();
            CreateMap<UserDTO, ShoppingUser>().ReverseMap();
            CreateMap<ClientUpdateDTO, ShoppingUser>().ReverseMap();
            CreateMap<UserForDropdownDTO, ShoppingUser>().ReverseMap();
            CreateMap<ICollection<UserForDropdownDTO>, IQueryable<ShoppingUser>>().ReverseMap();
            CreateMap<ICollection<UserForDropdownDTO>, ShoppingUser>().ReverseMap();
            CreateMap<ICollection<ShoppingUser>, IQueryable<ShoppingUser>>().ReverseMap();
 
            CreateMap<ClientUpdateDTO, EcommerceClient>().ReverseMap();
            CreateMap<ClientUpdateDTO, EcommerceEmployee>().ReverseMap();
            CreateMap<ClientRegisterRequestDTO, EcommerceClient>().ReverseMap();
            CreateMap<ClientRegisterRequestDTO, EcommerceEmployee>().ReverseMap();
            CreateMap<UserForDropdownDTO, EcommerceClient>().ReverseMap();
            CreateMap<UserRegisterDTO, EcommerceClient>().ReverseMap();
            CreateMap<UserRegisterDTO, EcommerceEmployee>().ReverseMap();
            CreateMap<UserDTO, EcommerceClient>().ReverseMap();
            CreateMap<UserDTO, EcommerceEmployee>().ReverseMap();
            CreateMap<EcommerceCompany, CompanyAddDTO>().ReverseMap();
            CreateMap<EcommerceProduct, EcommerceProductListDTO>().ReverseMap();
            CreateMap<EcommerceVariant, EcommerceVariantListDTO>().ReverseMap();
            CreateMap<EcommerceVariant, EcommerceVariantUpdateDTO>().ReverseMap();
            CreateMap<EcommerceVariant, EcommerceVariantGroupDTO>().ReverseMap();



            
            
            // Mapping for Refresh Tokens
            CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();

            // Mapping for Coach
            CreateMap<CoachRegisterDTO, Coach>().ReverseMap();
            CreateMap<CoachUpdateDTO, Coach>().ReverseMap();
            CreateMap<UserDTO, Coach>().ReverseMap();
            
            // Mapping for Student
            CreateMap<StudentRegisterDTO, Student>().ReverseMap();
            CreateMap<StudentUpdateDTO, Student>().ReverseMap();
            CreateMap<UserDTO, Student>().ReverseMap();

            // AuthenticatedUserResponseDto Mapping (for both Coach and Student)
            CreateMap<AuthenticatedUserResponseDto, Coach>().ReverseMap();
            CreateMap<AuthenticatedUserResponseDto, Student>().ReverseMap();

            // Mapping for Login
            CreateMap<LoginDto, Coach>().ReverseMap();
            CreateMap<LoginDto, Student>().ReverseMap();

            // Mapping for Reset Password
            CreateMap<ResetPasswordDTO, Coach>().ReverseMap();
            CreateMap<ResetPasswordDTO, Student>().ReverseMap();
            CreateMap<ResetPasswordLinkDTO, Coach>().ReverseMap();
            CreateMap<ResetPasswordLinkDTO, Student>().ReverseMap();

        }
    }
}
