using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Entities.Restourant;
using GTBack.Core.Entities.Shopping;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Shopping;

namespace GTBack.Service.Services.ShoppingService;

public class ShoppingCompanyService:IShoppingCompany
{
    private readonly IService<ShoppingCompany> _companyService;


    public ShoppingCompanyService(IService<ShoppingCompany> companyService)
    {
        _companyService = companyService;

    }

    public async Task<IResults> AddShoppingCompany(ShoppingCompanyAddDTO model)
    {
        var shoppingCompany = new ShoppingCompany()
        {
            Name = model.Name,
            Address = model.Address,
            Mail = model.Mail,
            Phone = model.Phone,
            Logo = model.Logo
        };
        await _companyService.AddAsync(shoppingCompany);
        return new SuccessResult();
    }
}