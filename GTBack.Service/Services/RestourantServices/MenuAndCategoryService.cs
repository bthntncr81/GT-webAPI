using System.Security.Claims;
using AutoMapper;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Entities.Restourant;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Restourant;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using XAct;

namespace GTBack.Service.Services.RestourantServices;

public class MenuAndCategoryService : IMenuAndCategoryService
{
    private readonly IService<Menu> _menuService;
    private readonly IService<MenuItem> _menuItemService;
    private readonly IService<Category> _categoryService;
    private readonly IService<ExtraMenuItem> _extraMenuItemService;
    private readonly ClaimsPrincipal? _loggedUser;
    private readonly IMapper _mapper;

    public MenuAndCategoryService(
        IHttpContextAccessor httpContextAccessor, IService<Menu> menuService, IService<MenuItem> menuItemService,
        IService<Category> categoryService, IService<ExtraMenuItem> extraMenuItemService,
        IMapper mapper)
    {
        _mapper = mapper;
        _menuService = menuService;
        _menuItemService = menuItemService;
        _categoryService = categoryService;
        _extraMenuItemService = extraMenuItemService;
        _loggedUser = httpContextAccessor.HttpContext?.User;
    }

    public async Task<IResults> MenuCreate(MenuCreateDTO model)
    {
        var response = _mapper.Map<Menu>(model);
        await _menuService.AddAsync(response);
        return new SuccessResult();
    }

    public async Task<IResults> CategoryAdd(CategoryAddOrUpdateDTO model)
    {
        if (model.Id != 0)
        {
            var response = _mapper.Map<Category>(model);
            await _categoryService.AddAsync(response);
            return new SuccessResult();
        }
        else
        {
            var response = _mapper.Map<Category>(model);
            await _categoryService.UpdateAsync(response);
            return new SuccessResult();
        }
    }

    public async Task<IResults> MenuItemAdd(MenuItemAddOrUpdateDTO model)
    {
        if (model.Id != 0)
        {
            var response = _mapper.Map<MenuItem>(model);
            await _menuItemService.AddAsync(response);
            return new SuccessResult();
        }
        else
        {
            var response = _mapper.Map<MenuItem>(model);
            await _menuItemService.UpdateAsync(response);
            return new SuccessResult();
        }
    }

    public async Task<IResults> ExtraMenuItemAdd(ExtraMenuItemAddOrUpdateDTO model)
    {
        if (model.Id != 0)
        {
            var response = _mapper.Map<ExtraMenuItem>(model);
            await _extraMenuItemService.AddAsync(response);
            return new SuccessResult();
        }
        else
        {
            var response = _mapper.Map<ExtraMenuItem>(model);
            await _extraMenuItemService.UpdateAsync(response);
            return new SuccessResult();
        }
    }

    public async Task<IResults> MenuDelete(long id)
    {
        var menu = await _menuService.Where(x => x.Id == id).FirstOrDefaultAsync();
        menu.IsDeleted = true;
        await _menuService.UpdateAsync(menu);
        return new SuccessResult();
    }

    public async Task<IResults> CategoryDelete(long id)
    {
        var category = await _categoryService.Where(x => x.Id == id).FirstOrDefaultAsync();
        category.IsDeleted = true;
        await _categoryService.UpdateAsync(category);
        return new SuccessResult();
    }

    public async Task<IResults> MenuItemDelete(long id)
    {
        var menu = await _menuItemService.Where(x => x.Id == id).FirstOrDefaultAsync();
        menu.IsDeleted = true;
        await _menuItemService.UpdateAsync(menu);
        return new SuccessResult();
    }

    public async Task<IResults> ExtraMenuItemDelete(long id)
    {
        var menu = await _extraMenuItemService.Where(x => x.Id == id).FirstOrDefaultAsync();
        menu.IsDeleted = true;
        await _extraMenuItemService.UpdateAsync(menu);
        return new SuccessResult();
    }

    public int? GetLoggedCompanyId()
    {
        var userRoleString = _loggedUser.FindFirstValue("companyId");
        if (int.TryParse(userRoleString, out var userId))
        {
            return userId;
        }

        return null;
    }

    public async Task<IDataResults<ICollection<CategoryListDTO>>> AllCategoryListByCompanyId()
    {
        var companyId = GetLoggedCompanyId();
        var menu = await _menuService.Where(x => x.RestoCompanyId == companyId).FirstOrDefaultAsync();
        var categories = await _categoryService.Where(x => x.MenuId == menu.Id && !x.IsDeleted).ToListAsync();

        var response = _mapper.Map<ICollection<CategoryListDTO>>(categories);
        return new SuccessDataResult<ICollection<CategoryListDTO>>(response);
    }


    public async Task<IDataResults<BaseListDTO<MenuItemListDTO, MenuListFilterRespresent>>> MenuItemListByCategoryId(
        BaseListFilterDTO<MenuListFilterDTO> menuFilter, long categoryId)
    {
        var query = _menuItemService.Where(x => x.CategoryId == categoryId && !x.IsDeleted);
        BaseListDTO<MenuItemListDTO, MenuListFilterRespresent> menuList =
            new BaseListDTO<MenuItemListDTO, MenuListFilterRespresent>();


        if (!CollectionUtilities.IsNullOrEmpty(menuFilter.RequestFilter.Name))
        {
            query = query.Where(x => x.Name.Contains(menuFilter.RequestFilter.Name));
        }

        if (!CollectionUtilities.IsNullOrEmpty(menuFilter.RequestFilter.Contains))
        {
            query = query.Where(x => x.Contains.Contains(menuFilter.RequestFilter.Contains));
        }

        if (!CollectionUtilities.IsNullOrEmpty(menuFilter.RequestFilter.Description))
        {
            query = query.Where(x => x.Description.Contains(menuFilter.RequestFilter.Description));
        }

        if (!ObjectExtensions.IsNull(menuFilter.RequestFilter.Price))
        {
            query = query.Where(x =>
                x.Price < menuFilter.RequestFilter.Price.Max && x.Price > menuFilter.RequestFilter.Price.Min);
        }

        if (!ObjectExtensions.IsNull(menuFilter.RequestFilter.Stock))
        {
            query = query.Where(x =>
                x.Stock < menuFilter.RequestFilter.Stock.Max && x.Stock > menuFilter.RequestFilter.Stock.Min);
        }


        menuList.List = _mapper.Map<ICollection<MenuItemListDTO>>(await query.ToListAsync());
        menuList.Filter = new MenuListFilterRespresent();
        return new SuccessDataResult<BaseListDTO<MenuItemListDTO, MenuListFilterRespresent>>(menuList);
    }

    public async Task<IDataResults<ICollection<ExtraMenuItemListDTO>>> ExtraMenuItemByMenuItemId(long menuItemId)
    {
        var menuItems = await _extraMenuItemService.Where(x => x.MenuItemId == menuItemId && !x.IsDeleted)
            .ToListAsync();
        var response = _mapper.Map<ICollection<ExtraMenuItemListDTO>>(menuItems);
        return new SuccessDataResult<ICollection<ExtraMenuItemListDTO>>(response);
    }

    public async Task<IDataResults<BaseListDTO<MenuItemListDTO, MenuListFilterRespresent>>> AllMenuItemsByCompanyId(
        BaseListFilterDTO<MenuListFilterDTO> menuFilter)
    {
        List<MenuItem> menuItems = new List<MenuItem>();
        var companyId = GetLoggedCompanyId();
        var menuRepo =  _menuService.Where(x =>  !x.IsDeleted);
        var categoryRepo = _categoryService.Where(x => !x.IsDeleted);
        var menuItemRepo = _menuItemService.Where(x => !x.IsDeleted);

        var query = from menu in menuRepo.Where(x=>x.RestoCompanyId==companyId)
            join category in categoryRepo on menu.Id equals category.MenuId into categoryLeft
            from category in categoryLeft
            join menuItem in menuItemRepo on category.Id equals menuItem.CategoryId into menuItemLeft
            from menuItem in menuItemLeft
            select new MenuItemListDTO()

            {
                Id = menuItem.Id.IsNull()? 0 :menuItem.Id,
                Name = menuItem.Name,
                Price = menuItem.Price,
                Stock = menuItem.Stock,
                Image = menuItem.Image,
                Description = menuItem.Description,
                Contains = menuItem.Contains,
                CategoryId = category.Id,
            };

        var myCheckList = await query.ToListAsync();

        if (!CollectionUtilities.IsNullOrEmpty(menuFilter.RequestFilter.Name))
        {
            query = query.Where(x => x.Name.Contains(menuFilter.RequestFilter.Name));
        }

        if (!CollectionUtilities.IsNullOrEmpty(menuFilter.RequestFilter.Contains))
        {
            query = query.Where(x => x.Contains.Contains(menuFilter.RequestFilter.Contains));
        }

        if (!CollectionUtilities.IsNullOrEmpty(menuFilter.RequestFilter.Description))
        {
            query = query.Where(x => x.Description.Contains(menuFilter.RequestFilter.Description));
        }

        if (!ObjectExtensions.IsNull(menuFilter.RequestFilter.Price))
        {
            query = query.Where(x =>
                x.Price < menuFilter.RequestFilter.Price.Max && x.Price > menuFilter.RequestFilter.Price.Min);
        }

        if (!ObjectExtensions.IsNull(menuFilter.RequestFilter.Stock))
        {
            query = query.Where(x =>
                x.Stock < menuFilter.RequestFilter.Stock.Max && x.Stock > menuFilter.RequestFilter.Stock.Min);
        }

        query = query.Skip(menuFilter.PaginationFilter.Skip).Take(menuFilter.PaginationFilter.Take);
        

        BaseListDTO<MenuItemListDTO, MenuListFilterRespresent> menuList =
            new BaseListDTO<MenuItemListDTO, MenuListFilterRespresent>();

        menuList.List = _mapper.Map<ICollection<MenuItemListDTO>>(await query.ToListAsync());
        menuList.Filter = new MenuListFilterRespresent();

        return new SuccessDataResult<BaseListDTO<MenuItemListDTO, MenuListFilterRespresent>>(menuList);
    }
}