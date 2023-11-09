using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Results;

namespace GTBack.Core.Services.Restourant;

public interface IDepartmentService<T1,T2>:IGenericService<DepartmentAddDTO,DepartmentListDTO>
{
    Task<IDataResults<ICollection<T2>>> ListByCompanyId(long companyId);

}