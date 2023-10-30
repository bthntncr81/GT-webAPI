using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Entities.Restourant;

namespace GTBack.Service.Mapping.Resourant;

public class RestourantMapProfile:Profile
{
    
        public RestourantMapProfile()
        {
            //CLİENT
            CreateMap<Client, ClientRegisterRequestDTO>().ReverseMap();
            CreateMap<Client, UserDTO>().ReverseMap();
            
            //EMPLOYEE
            CreateMap<Employee, EmployeeRegisterDTO>().ReverseMap();
            
            //ROLE
            CreateMap<RoleListDTO, Role>().ReverseMap();
            CreateMap<ICollection<RoleListDTO>, IQueryable<Role>>().ReverseMap();
            CreateMap<Role, RoleCreateDTO>().ReverseMap();
            
            //DEPARTMENT
            CreateMap<DepartmentListDTO, Department>().ReverseMap();
            CreateMap<ICollection<DepartmentListDTO>, IQueryable<Department>>().ReverseMap();
            CreateMap<Department, DepartmentAddDTO>().ReverseMap();
        }
    
}