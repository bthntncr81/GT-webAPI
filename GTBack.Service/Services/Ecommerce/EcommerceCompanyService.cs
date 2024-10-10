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
using XAct;

namespace GTBack.Service.Services.Ecommerce;

public class EcommerceCompanyService : IEcommerceCompanyService
{
    private readonly IService<EcommerceCompany> _companyService;
    private readonly IMapper _mapper;


    public EcommerceCompanyService(IService<EcommerceCompany> companyService, IMapper mapper)
    {
        _companyService = companyService;
        _mapper = mapper;

    }

    public async Task<IResults> AddShoppingCompany(CompanyAddDTO model)
    {
        var company = await _companyService.Where(x => x.Id == model.Id).FirstOrDefaultAsync();


        if (company.IsNull())
        {
            var shoppingCompany = new EcommerceCompany()
            {
                Id = model.Id,
                Logo = model.Logo,
                WebAddress = model.WebAddress,
                Name = model.Name,
                SmtpPort = model.SmtpPort,
                SmtpServer = model.SmtpServer,
                EmailPassword = model.EmailPassword,
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

        }
        else
        {
            company.Id = model.Id;
            company.WebAddress = model.WebAddress;
            company.Logo = model.Logo;
            company.Name = model.Name;
            company.SmtpPort = model.SmtpPort;
            company.SmtpServer = model.SmtpServer;
            company.EmailPassword = model.EmailPassword;
            company.Address = model.Address;
            company.Email = model.Email;
            company.Phone = model.Phone;
            company.GeoCodeY = model.GeoCodeY;
            company.GeoCodeX = model.GeoCodeX;
            company.ThemeId = model.ThemeId;
            company.PrimaryColor = model.PrimaryColor;
            company.SecondaryColor = model.SecondaryColor;
            company.VergiNumber = model.VergiNumber;
            company.IyzicoClientId = model.IyzicoClientId;
            company.IyzicoSecretId = model.IyzicoSecretId;
            company.PrivacyPolicy = model.PrivacyPolicy;
            company.DeliveredAndReturnPolicy = model.DeliveredAndReturnPolicy;
            company.DistanceSellingContract = model.DistanceSellingContract;

            await _companyService.UpdateAsync(company);

        }



        return new SuccessResult();
    }


    public async Task<IDataResults<CompanyAddDTO>> GetCompany(int id)
    {
        var company = await _companyService.Where(x => x.Id == id).FirstOrDefaultAsync();
        var companyDto = _mapper.Map<CompanyAddDTO>(company);
        return new SuccessDataResult<CompanyAddDTO>(companyDto);

    }

}