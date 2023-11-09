using GTBack.Core.DTO;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Results;

namespace GTBack.Core.Services.Restourant;

public interface IEmployeeService
{
 
    Task<IDataResults<EmployeeListDTO>> GetById(int id);
    Task<IDataResults<BaseListDTO<EmployeeListDTO,EmployeeFilterRepresent>>> ListEmployee(BaseListFilterDTO<EmployeeListFilter> filter);
    Task<IDataResults<AuthenticatedUserResponseDto>> Login(LoginRestourantDTO loginDto);
    Task<IDataResults<AuthenticatedUserResponseDto>> PasswordChoose(PasowordConfirmDTO loginDto);
    Task<IResults> Register(EmployeeRegisterDTO registerDto);
    Task<IResults> EmployeeRoleChange(EmployeeRegisterDTO registerDto);
}