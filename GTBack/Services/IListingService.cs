using System.Linq.Expressions;
using GTBack.Core.DTO;

namespace GTBack.Core.Services;

public interface IListingServiceI<T1,T2> where T1 : class where T2:class
{
    Task<IQueryable<T1>> GenericListFilter(BaseListFilterDTO<T2> filter);
    BaseListFilterDTO<T2> CacheKey(BaseListFilterDTO<T2> model);

}