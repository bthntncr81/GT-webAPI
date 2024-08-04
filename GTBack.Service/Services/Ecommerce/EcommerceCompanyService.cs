using AutoMapper;
using GTBack.Core.DTO.Ecommerce;
using GTBack.Core.DTO.Ecommerce.Response;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Entities.Ecommerce;
using GTBack.Core.Entities.Restourant;
using GTBack.Core.Entities.Shopping;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Shopping;
using Microsoft.EntityFrameworkCore;

namespace GTBack.Service.Services.Ecommerce;

public class EcommerceCompanyService:IEcommerceCompanyService
{
    private readonly IService<EcommerceCompany> _companyService;
    private readonly IMapper _mapper;


    public EcommerceCompanyService(IService<EcommerceCompany> companyService,IMapper mapper)
    {
        _companyService = companyService;
        _mapper = mapper;

    }

    public async Task<IResults> AddShoppingCompany(CompanyAddDTO model)
    {
        var shoppingCompany = new EcommerceCompany()
        {
            Logo = model.Logo,
            Name = model.Name,
            Address = model.Address,
            Email = model.Email,
            Phone = model.Phone,
            GeoCodeY = model.GeoCodeY,
            GeoCodeX = model.GeoCodeX,
            ThemeId = model.ThemeId,
            PrimaryColor = model.PrimaryColor,
            SecondaryColor = model.SecondaryColor,
            VergiNumber = model.VergiNumber,
            IyzicoClientId = model.IyzicoClientId,
            IyzicoSecretId = model.IyzicoSecretId,
            PrivacyPolicy = model.PrivacyPolicy,
            DeliveredAndReturnPolicy = model.DeliveredAndReturnPolicy,
            DistanceSellingContract = model.DistanceSellingContract,
        };
        
        await _companyService.AddAsync(shoppingCompany);
        return new SuccessResult();
    }


    public async Task<IDataResults<CompanyAddDTO>> GetCompany(int id)
    {
     var company= await _companyService.Where(x => x.Id == id).FirstOrDefaultAsync();
     var companyDto= _mapper.Map<CompanyAddDTO>(company);
     return new SuccessDataResult<CompanyAddDTO>(companyDto);
        
    }
    
}