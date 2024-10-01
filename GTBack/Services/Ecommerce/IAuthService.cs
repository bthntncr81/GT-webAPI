using System.Net.Mail;
using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Shopping;
using GTBack.Core.DTO.Shopping.Filter;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Results;


namespace GTBack.Core.Services.Ecommerce;

public interface IAuthService
{
    Task<IDataResults<ClientUpdateDTO>> Me();
    Task<IDataResults<UserDTO>> GetById(long id);
    Task<IDataResults<AuthenticatedUserResponseDto>> Login(LoginDto loginDto);
    Task<IDataResults<AuthenticatedUserResponseDto>> Register(ClientRegisterRequestDTO registerDto);
    Task<IDataResults<AuthenticatedUserResponseDto>> GoogleLogin(GoogleLoginDTO model);
    Task<IResults> ResetPasswordLink(ResetPasswordLinkDTO userMail);
    Task<IDataResults<ClientUpdateDTO>> UpdateUser(ClientUpdateDTO registerDto);
    Task<IResults> ResetPassword(ResetPasswordDTO password);


}