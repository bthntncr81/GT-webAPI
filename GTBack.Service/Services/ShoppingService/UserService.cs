using System.Net;
using System.Security.Claims;
using System.Xml.Serialization;
using AutoMapper;
using Google.Apis.Auth;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Shopping;
using GTBack.Core.DTO.Shopping.Filter;
using GTBack.Core.Entities.Shopping;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Shopping;
using GTBack.Service.Utilities;
using GTBack.Service.Utilities.Jwt;
using GTBack.Service.Validation.Restourant;
using GTBack.Service.Validation.Tool;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using XAct;

namespace GTBack.Service.Services.ShoppingService;

public class ShoppingUserService:IShoppingUserService
{
    
    private readonly IService<ShoppingUser> _service;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ClaimsPrincipal? _loggedUser;
    private readonly IMapper _mapper;
    private readonly  IJwtTokenService<BaseRegisterDTO> _tokenService;
    private readonly IMemoryCache _cache;

    public ShoppingUserService(IMemoryCache cache,IRefreshTokenService refreshTokenService,  IJwtTokenService<BaseRegisterDTO> tokenService,
        IHttpContextAccessor httpContextAccessor, IService<ShoppingUser> service,
         IMapper mapper)
    {
        _mapper = mapper;
        _cache = cache;
        _service = service;
        _loggedUser = httpContextAccessor.HttpContext?.User;
        _refreshTokenService = refreshTokenService;
        _tokenService = tokenService;
    }

    //GET CLİENT BY ID METHOD
    public async Task<IDataResults<UserDTO>> GetById(long id)
    {
        var place = await _service.GetByIdAsync(x => x.Id == id);
        var data = _mapper.Map<UserDTO>(place);
        return new SuccessDataResult<UserDTO>(data);
    }
    

    
    //DELETE CLİENT BY ID METHOD
    public async Task<IResults> Delete(long id)
    {
        var client = await _service.GetByIdAsync(x => x.Id == id);
        client.IsDeleted = true;
        await _service.UpdateAsync(client);
        return new SuccessResult();
    }
    
    
    public  async Task<List<ProductTarzYeri>> XmlConverter(string xmlContent,BpmFilter filter)
    {

        var control = new CacheControlObject()
        {
            XmlContent = xmlContent,
            filter = filter
        };
        var resource = await _cache.GetOrCreateAsync(control, async entry =>
        {

            XmlSerializer serializer = new XmlSerializer(typeof(ProductsTarzYeri));
            using (StringReader reader = new StringReader(xmlContent))
            {


                ProductsTarzYeri myObject = (ProductsTarzYeri)serializer.Deserialize(reader);

                var elem = myObject.ProductList.Where(x => float.Parse(x.price) != 0).ToList();

                if (filter.MainCategory == "kadin")
                {
                    filter.MainCategory = "kadın";
                }

                if (!filter.Id.IsNullOrEmpty())
                {
                    elem = elem.Where(x => x.id.ToLower().Equals(filter.Id)).ToList();

                }

                if (!filter.MainCategory.IsNullOrEmpty())
                {
                    elem = elem.Where(x => x.main_category.ToLower().Contains(filter.MainCategory)).ToList();

                }

                if (!filter.SubCategory.IsNullOrEmpty())
                {
                    elem = elem.Where(x => x.sub_category.ToLower().Contains(filter.SubCategory)).ToList();
                }


                return elem;
            }
        });
        return resource;
    }
    
    public async  Task<List<ProductBPM.ElementBpm>> XmlConverterBpm(string xmlContent,BpmFilter filter)
    {

        var control = new CacheControlObject()
        {
    XmlContent = xmlContent,
    filter = filter
        };
        var resource = await _cache.GetOrCreateAsync(control, async entry =>
        {

            XmlSerializer serializer = new XmlSerializer(typeof(ProductBPM.ProductBpms));
            using (StringReader reader = new StringReader(xmlContent))
            {
                ProductBPM.ProductBpms myObject = (ProductBPM.ProductBpms)serializer.Deserialize(reader);


                if (filter.MainCategory == "kadin")
                {
                    filter.MainCategory = "kadın";
                }

                var elem = myObject.ProductList.Where(x =>
                    Int32.Parse(x.Stock) != 0 && !(float.Parse(x.Price) == 0 && float.Parse(x.Price2) == 0)).ToList();

                if (!filter.Id.IsNullOrEmpty())
                {
                    elem = elem.Where(x => x.Product_id.ToLower().Equals(filter.Id)).ToList();
                }

                if (!filter.MainCategory.IsNullOrEmpty())
                {
                    elem = elem.Where(x => x.MainCategory.ToLower().Contains(filter.MainCategory)).ToList();
                }

                if (!filter.SubCategory.IsNullOrEmpty())
                {
                    elem = elem.Where(x =>
                        x.SubCategory.ToLower().Contains(filter.SubCategory) ||
                        x.Category.ToLower().Contains(filter.SubCategory)).ToList();
                }

                return elem;
            }

        });
        ;
        return resource;

    }
    
    
    ///Register CLİENT Method  we use validation TO DO:BATUHAN --> VALDATİONS DONE
    public async Task<IDataResults<AuthenticatedUserResponseDto>> Register(ClientRegisterRequestDTO registerDto)
    {
        var valResult =
            FluentValidationTool.ValidateModelWithKeyResult<ClientRegisterRequestDTO>(new ClientRegisterValidator(), registerDto);
        
        if (valResult.Success == false)
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>(HttpStatusCode.BadRequest, valResult.Errors);
        }
    
        var mail = registerDto.Mail.ToLower().Trim();
        var user = await _service.GetByIdAsync((x => x.Mail.ToLower() == mail && !x.IsDeleted)); //get by mail eklenecek


        if (user != null)
        {
            valResult.Errors.Add("", Messages.User_Email_Exists);
            return new ErrorDataResults<AuthenticatedUserResponseDto>(HttpStatusCode.BadRequest, valResult.Errors);
        }


        user = new ShoppingUser()
        {
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            Mail = registerDto.Mail,
            Address = registerDto.Address,
            Surname = registerDto.Surname,
            Phone = registerDto.Phone,
            IsDeleted = false,
            Name = registerDto.Name,
            ShoppingCompanyId = registerDto.CompanyId,
            UserTypeId = registerDto.UserTypeShoppingId,
            PasswordHash = SHA1.Generate(registerDto.Password)
        };

        await _service.AddAsync(user);

        var response = await Authenticate(_mapper.Map<ClientRegisterRequestDTO>(user));
        return new SuccessDataResult<AuthenticatedUserResponseDto>(response, HttpStatusCode.OK);
    }

    public long? GetLoggedUserId()
    {
        var userRoleString = _loggedUser.FindFirstValue("Id");
        if (long.TryParse(userRoleString, out var userId))
        {
            return userId;
        }

        return null;
    }

    public async Task<IDataResults<UserDTO>> Me()
    {
        var userId = GetLoggedUserId();

        UserDTO? user = null;


        var userModel = await _service.FindAsNoTrackingAsync(x => !x.IsDeleted && x.Id == userId);
        user = _mapper.Map<UserDTO>(userModel);


        if (user == null)
        {
            return new ErrorDataResults<UserDTO>(Messages.User_NotFound_Message, HttpStatusCode.NotFound);
        }

        return new SuccessDataResult<UserDTO>(user);
    }

  

    public async Task<IDataResults<AuthenticatedUserResponseDto>> Login(LoginDto loginDto)
    {
        var valResult =
            FluentValidationTool.ValidateModelWithKeyResult(new ClientLoginValidator(), loginDto);
        if (valResult.Success == false)
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>(HttpStatusCode.BadRequest, valResult.Errors);
        }

        var mail = loginDto.Mail.ToLower().Trim();
        var parent =
            await _service.GetByIdAsync((x => x.Mail.ToLower() == mail && !x.IsDeleted)); //get by mail eklenecek


        if (parent?.PasswordHash == null)
        {
            valResult.Errors.Add("", Messages.User_NotFound_Message);
            return new ErrorDataResults<AuthenticatedUserResponseDto>(Messages.User_NotFound_Message,
                HttpStatusCode.BadRequest);
        }

        if (!Utilities.SHA1.Verify(loginDto.Password, parent.PasswordHash))
        {
            valResult.Errors.Add("", Messages.User_Login_Message_Notvalid);
            return new ErrorDataResults<AuthenticatedUserResponseDto>(Messages.Password_Wrong,
                HttpStatusCode.BadRequest);
        }

        var response = await Authenticate(_mapper.Map<ClientRegisterRequestDTO>(parent));
        return new SuccessDataResult<AuthenticatedUserResponseDto>(response);
    }

    private async Task<AuthenticatedUserResponseDto> Authenticate(ClientRegisterRequestDTO userDto)
    {
        var accessToken = _tokenService.GenerateAccessToken(userDto);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenDto = new RefreshTokenDto()
        {
            Token = refreshToken,
        };
        await _refreshTokenService.Create(refreshTokenDto);

        return new AuthenticatedUserResponseDto()
        {
            AccessToken = accessToken.Value,
            AccessTokenExpirationTime = accessToken.ExpirationTime,
            RefreshToken = refreshToken
        };
    }
    
    public async Task<IDataResults<AuthenticatedUserResponseDto>> GoogleLogin (GoogleLoginDTO model)
    {
        GoogleJsonWebSignature.ValidationSettings? settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>()
                { "1067621219285-fukuebsj13aa2b611b4fcs2j7s447kl6.apps.googleusercontent.com" }
        };
        GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken, settings);

        UserLoginInfo userLoginInfo = new(model.Provider, payload.Subject, model.Provider);

        var user = await _service.FindAsync(x => x.Mail == payload.Email);
        bool result = user != null;


        if (!result)
        {
            return new ErrorDataResults<AuthenticatedUserResponseDto>(Messages.User_NotFound_Message,
                HttpStatusCode.BadRequest);
        }
        

        var response = await Authenticate(_mapper.Map<ClientRegisterRequestDTO>(user));
        return new SuccessDataResult<AuthenticatedUserResponseDto>(response);
    }
}