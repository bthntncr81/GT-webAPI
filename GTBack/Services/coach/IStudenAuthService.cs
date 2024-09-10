using GTBack.Core.DTO;
using GTBack.Core.DTO.Coach.Request;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Results;

namespace GTBack.Core.Services.coach;

public interface IStudentAuthService
{
    Task<IDataResults<UserDTO>> GetById(long id);
    Task<IDataResults<AuthenticatedUserResponseDto>> Register(StudentRegisterDTO registerDto);
    Task<IDataResults<AuthenticatedUserResponseDto>> Login(LoginDto loginDto);
    Task<IResults> Delete(long id);
    Task<IDataResults<StudentUpdateDTO>> UpdateStudent(StudentUpdateDTO updateDto);
    Task<IResults> ResetPassword(ResetPasswordDTO resetPasswordDto);
    Task<IResults> ResetPasswordLink(ResetPasswordLinkDTO resetPasswordLinkDto);
}
