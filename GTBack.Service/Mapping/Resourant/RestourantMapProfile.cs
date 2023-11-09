using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant;
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
            CreateMap<EmployeeListDTO, Employee>().ReverseMap();
            CreateMap<ICollection<EmployeeListDTO>, IQueryable<Employee>>().ReverseMap();
            
            //ROLE
            CreateMap<RoleListDTO, Role>().ReverseMap();
            CreateMap<ICollection<RoleListDTO>, IQueryable<Role>>().ReverseMap();
            CreateMap<Role, RoleCreateDTO>().ReverseMap();
            CreateMap<ICollection<RoleList>, IQueryable<EmployeeRoleRelation>>().ReverseMap();
            CreateMap<RoleList,EmployeeRoleRelation>().ReverseMap();
            
            //DEPARTMENT
            CreateMap<DepartmentListDTO, Department>().ReverseMap();
            CreateMap<ICollection<DepartmentListDTO>, IQueryable<Department>>().ReverseMap();
            CreateMap<Department, DepartmentAddDTO>().ReverseMap();
            
            //MENU
            CreateMap<Menu, MenuCreateDTO>().ReverseMap();
            
            //CATEGORY
            CreateMap<CategoryListDTO, Category>().ReverseMap();
            CreateMap<ICollection<CategoryListDTO>, IQueryable<Category>>().ReverseMap();
            CreateMap<Category, CategoryAddOrUpdateDTO>().ReverseMap();
            
            //MENUITEM
            CreateMap<MenuItemListDTO, MenuItem>().ReverseMap();
            CreateMap<ICollection<MenuItemListDTO>, IQueryable<MenuItem>>().ReverseMap();
            CreateMap<MenuItem, MenuItemAddOrUpdateDTO>().ReverseMap();
            
            //EXTRAMENUITEM
            CreateMap<ExtraMenuItemListDTO, ExtraMenuItem>().ReverseMap();
            CreateMap<ICollection<ExtraMenuItemListDTO>, IQueryable<ExtraMenuItem>>().ReverseMap();
            CreateMap<ExtraMenuItem, ExtraMenuItemAddOrUpdateDTO>().ReverseMap();
            
            //TABLEAREA
            CreateMap<TableAreaListDTO, TableArea>().ReverseMap();
            CreateMap<ICollection<TableAreaListDTO>, IQueryable<TableArea>>().ReverseMap();
            CreateMap<TableArea, TableAreaAddOrUpdateDTO>().ReverseMap();
            
            //TABLE
            CreateMap<TableListDTO, Table>().ReverseMap();
            CreateMap<ICollection<TableListDTO>, IQueryable<Table>>().ReverseMap();
            CreateMap<Table, TableAddOrUpdateDTO>().ReverseMap();
            
            //ADDİTİON
            CreateMap<AdditionListDTO, Addition>().ReverseMap();
            CreateMap<ICollection<AdditionListDTO>, IQueryable<Addition>>().ReverseMap();
            CreateMap<Addition, AdditionAddOrUpdateDTO>().ReverseMap();
            
            //ORDER
            CreateMap<OrderListDTO, Order>().ReverseMap();
            CreateMap<ICollection<OrderListDTO>, IQueryable<Order>>().ReverseMap();
            CreateMap<Order, OrderAddOrUpdateDTO>().ReverseMap();
        }
    
}