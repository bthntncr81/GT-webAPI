using GTBack.Core.Results;

namespace GTBack.Core.Services;

public interface IGenericService<T1,T2>
{
    Task<IDataResults<T1>> Create(T1 model);
    Task<IDataResults<T1>> Update(T1 model,long id);
    Task<IResults> Delete(long id);
    Task<IDataResults<ICollection<T2>>> List();
}
