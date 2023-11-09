using GTBack.Core.DTO;
using GTBack.Core.Entities;
using GTBack.Core.Results;

namespace GTBack.Core.Services;

public interface IUserService
{
    // Task<IDataResults<ICollection<UserDTO>>> List(CustomerListParameters parameters);
    
    // Task<IResults> Put(UpdateCustomer place);


  

    Task<IDataResults<UserDTO>> GetById(int id);
    Task<IDataResults<ICollection<UserForDropdownDTO>>> AdminListByCompanyId(int companyId);
    Task<IDataResults<UserDTO>> Me();
    int? GetLoggedUserId();
    Task<IDataResults<AuthenticatedUserResponseDto>> Login(LoginDto loginDto);
    Task<IResults> Delete(int id);
    Task<IDataResults<AuthenticatedUserResponseDto>> Register(UserRegisterDTO registerDto);
    Task<IDataResults<AuthenticatedUserResponseDto>> GoogleLogin(GoogleLoginDTO model);



}

