using AutoMapper;
using GTBack.Core.DTO;
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

    public MenuController( IMapper mapper,IMenuAndCategoryService service)
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
    
    [HttpPost("MenuItemListByCategoryId")]
    public async Task<IActionResult> MenuItemListByCategoryId([FromBody]BaseListFilterDTO<MenuListFilterDTO>  menuFilter,long categoryId)
    {
        return ApiResult(await _service.MenuItemListByCategoryId(menuFilter,categoryId));
    }
    [Authorize]
    [HttpPost("AllMenuItems")]
    public async Task<IActionResult> AllMenuItemsByCompanyId( BaseListFilterDTO<MenuListFilterDTO> menuFilter)
    {
        return ApiResult(await _service.AllMenuItemsByCompanyId(menuFilter));
    }
    
    [HttpPost("ExtraMenuItemByMenuItemId")]
    public async Task<IActionResult> ExtraMenuItemByMenuItemId(long menuItemId)
    {
        return ApiResult(await _service.ExtraMenuItemByMenuItemId(menuItemId));
    }
    
    [HttpDelete("DeleteCategory")]
    public async Task<IActionResult> DeleteCategory(long id)
    {
        return ApiResult(await _service.CategoryDelete(id));
    }
    
    [HttpDelete("DeleteMenuItem")]
    public async Task<IActionResult> DeleteMenuItem(long id)
    {
        return ApiResult(await _service.MenuItemDelete(id));
    }
    
    [HttpDelete("DeleteExtraMenuItem")]
    public async Task<IActionResult> DeleteExtraMenuItem(long id)
    {
        return ApiResult(await _service.ExtraMenuItemDelete(id));
    }
    
   
}