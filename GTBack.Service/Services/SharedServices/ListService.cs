using GTBack.Core;
using GTBack.Core.DTO;
using GTBack.Core.Enums.Restourant;
using GTBack.Core.Repositories;
using GTBack.Core.Services;
using GTBack.Core.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using XAct;

namespace GTBack.Service.Services.SharedServices;

public class ListService<T1,T2>:IListingServiceI<T1,T2> where T1 :BaseEntity where T2 : class
{
    
    private readonly IGenericRepository<T1> _repository;

    
    private readonly IUnitOfWork _unitOfWork;

    public ListService(IUnitOfWork unitOfWork, IGenericRepository<T1> repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }
    
    public async Task<IQueryable<T1>> GenericListFilter(BaseListFilterDTO<T2> filter)
    {
        var query = _repository.Where(x=>!x.IsDeleted);
        
        
        foreach (var property in typeof(T2).GetProperties())
        {
            var name = property.Name;
            var myObject = filter.RequestFilter.GetType().GetProperty(name).GetValue(filter.RequestFilter, null);
            if(myObject!=null)
            {
                    query =  query.Where(x=>x.GetType().GetProperty(name).GetValue(x,null)==myObject);
            }
        }
        return query;
    }
    
    
    public  BaseListFilterDTO<T2> CacheKey(BaseListFilterDTO<T2> model)
    {
        return model;
    }
}