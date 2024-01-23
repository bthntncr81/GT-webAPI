using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.Results;

namespace GTBack.Core.Services.Shopping;

public interface IShoppingUserService
{
    Task<IDataResults<UserDTO>> Me();
    Task<IDataResults<UserDTO>> GetById(long id);
    Task<IDataResults<AuthenticatedUserResponseDto>> Login(LoginDto loginDto);
    Task<IDataResults<AuthenticatedUserResponseDto>> Register(ClientRegisterRequestDTO registerDto);
    Task<IDataResults<AuthenticatedUserResponseDto>> GoogleLogin(GoogleLoginDTO model);

}