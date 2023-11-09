using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Results;

namespace GTBack.Core.Services.Restourant;

public interface IMenuAndCategoryService
{
    Task<IResults> MenuCreate(MenuCreateDTO model);
    Task<IResults> CategoryAdd(CategoryAddOrUpdateDTO model);
    Task<IResults> MenuItemAdd(MenuItemAddOrUpdateDTO model);
    Task<IResults> ExtraMenuItemAdd(ExtraMenuItemAddOrUpdateDTO model);
    Task<IResults> MenuDelete(long id);
    Task<IResults> CategoryDelete(long id);
    Task<IResults> MenuItemDelete(long id);
    Task<IResults> ExtraMenuItemDelete(long id);
    Task<IDataResults<ICollection<CategoryListDTO>>> AllCategoryListByCompanyId();
    Task<IDataResults<BaseListDTO<MenuItemListDTO,MenuListFilterRespresent>>> MenuItemListByCategoryId(BaseListFilterDTO<MenuListFilterDTO>  menuFilter,long categoryId);
    Task<IDataResults<ICollection<ExtraMenuItemListDTO>>> ExtraMenuItemByMenuItemId(long menuItemId);

    Task<IDataResults<BaseListDTO<MenuItemListDTO, MenuListFilterRespresent>>> AllMenuItemsByCompanyId(
        BaseListFilterDTO<MenuListFilterDTO> menuFilter);

}