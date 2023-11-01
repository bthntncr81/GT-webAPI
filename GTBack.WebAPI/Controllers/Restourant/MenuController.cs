using AutoMapper;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Services;
using GTBack.Core.Services.Restourant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GTBack.WebAPI.Controllers.Restourant;

public class MenuController: CustomRestourantBaseController
{
    private readonly IMapper _mapper;
    private readonly IMenuAndCategoryService _service;

    public MenuController( IMapper mapper,IRoleService<RoleCreateDTO,RoleListDTO> roleService,IMenuAndCategoryService service)
    {
        _service = service;
        _mapper = mapper;
    }
    
         
    [HttpPost("CreatMenu")]
    public async Task<IActionResult> CreateMenu(MenuCreateDTO model)
    {
        return ApiResult(await _service.MenuCreate(model));
    }
    
   
    
    [HttpPost("CategoryAdd")]
    public async Task<IActionResult> CategoryAddOrUpdate(CategoryAddOrUpdateDTO model)
    {
        return ApiResult(await _service.CategoryAdd(model));
    }
    
    [HttpPost("MenuItemAdd")]
    public async Task<IActionResult> MenuItemAddOrUpdate(MenuItemAddOrUpdateDTO model)
    {
        return ApiResult(await _service.MenuItemAdd(model));
    }
    
  
    [Authorize]
    [HttpGet("CategoryList")]
    public async Task<IActionResult> CategoryList()
    {
        return ApiResult(await _service.AllCategoryListByCompanyId());
    }
    
    [HttpGet("MenuItemListByCategoryId")]
    public async Task<IActionResult> MenuItemListByCategoryId(int categoryId)
    {
        return ApiResult(await _service.MenuItemListByCategoryId(categoryId));
    }
    [Authorize]
    [HttpGet("MenuItemListByCompanyId")]
    public async Task<IActionResult> MenuItemListByCompanyId()
    {
        return ApiResult(await _service.AllMenuItemsByCompanyId());
    }
    
    [HttpPost("ExtraMenuItemByMenuItemId")]
    public async Task<IActionResult> ExtraMenuItemByMenuItemId(int menuItemId)
    {
        return ApiResult(await _service.ExtraMenuItemByMenuItemId(menuItemId));
    }
    
    [HttpDelete("DeleteCategory")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        return ApiResult(await _service.CategoryDelete(id));
    }
    
    [HttpDelete("DeleteMenuItem")]
    public async Task<IActionResult> DeleteMenuItem(int id)
    {
        return ApiResult(await _service.MenuItemDelete(id));
    }
    
    [HttpDelete("DeleteExtraMenuItem")]
    public async Task<IActionResult> DeleteExtraMenuItem(int id)
    {
        return ApiResult(await _service.ExtraMenuItemDelete(id));
    }
    
   
}