using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Shopping;
using GTBack.Core.DTO.Shopping.Filter;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Results;

namespace GTBack.Core.Services.Shopping;

public interface IShoppingUserService
{
    Task<IDataResults<ClientUpdateDTO>> Me();
    Task<IDataResults<UserDTO>> GetById(long id);
    Task<IDataResults<AuthenticatedUserResponseDto>> Login(LoginDto loginDto);
    Task<IDataResults<AuthenticatedUserResponseDto>> Register(ClientRegisterRequestDTO registerDto);
    Task<IDataResults<AuthenticatedUserResponseDto>> GoogleLogin(GoogleLoginDTO model);

    Task< List<ProductTarzYeri>>XmlConverter(string xmlContent,BpmFilter filter);
    Task<List<ProductBPM.ElementBpm>> XmlConverterBpm(string xmlContent, BpmFilter filter);

    Task<IResults> ResetPasswordLink(ResetPasswordLinkDTO userMail);

    Task<IDataResults<ClientUpdateDTO>> UpdateUser(ClientUpdateDTO registerDto);

    Task<IResults> ResetPassword(ResetPasswordDTO password);

}