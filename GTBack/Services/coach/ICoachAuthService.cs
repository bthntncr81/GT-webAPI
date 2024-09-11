using GTBack.Core.DTO;
using GTBack.Core.DTO.Coach.Request;
using GTBack.Core.DTO.Shopping.Request;
using GTBack.Core.Results;

namespace GTBack.Core.Services.coach;

public interface ICoachAuthService

{
    Task<IDataResults<UserDTO>> GetById(long id);
    Task<IDataResults<AuthenticatedUserResponseDto>> Register(CoachRegisterDTO registerDto);
    Task<IDataResults<AuthenticatedUserResponseDto>> Login(LoginDto loginDto);
    Task<IResults> Delete(long id);
    Task<IDataResults<CoachUpdateDTO>> UpdateCoach(CoachUpdateDTO updateDto);
    Task<IResults> ResetPassword(ResetPasswordDTO resetPasswordDto);
    Task<IResults> ResetPasswordLink(ResetPasswordLinkDTO resetPasswordLinkDto);

    Task<IResults> CreateCoachGuid();
}