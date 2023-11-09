using System.Security.Claims;
using AutoMapper;
using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Entities.Restourant;
using GTBack.Core.Results;
using GTBack.Core.Services;
using GTBack.Core.Services.Restourant;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GTBack.Service.Services.RestourantServices;

public class TableAndAreaService : ITableAndAreaService
{
    private readonly IService<Table> _tableService;
    private readonly IService<TableArea> _tableAreaService;
    private readonly IMapper _mapper;
    private readonly ClaimsPrincipal? _loggedUser;


    public TableAndAreaService(
        IHttpContextAccessor httpContextAccessor, IService<Table> tableService, IService<TableArea> tableAreaService,
        IMapper mapper)
    {
        _mapper = mapper;
        _tableService = tableService;
        _tableAreaService = tableAreaService;
        _loggedUser = httpContextAccessor.HttpContext?.User;

    }

    public async Task<IResults> TableAreaAddOrUpdate(TableAreaAddOrUpdateDTO model)
    {
        var response = _mapper.Map<TableArea>(model);
        if (model.Id == 0)
        {
            await _tableAreaService.AddAsync(response);
            return new SuccessResult();
        }
        else
        {
            await _tableAreaService.UpdateAsync(response);
            return new SuccessResult();
        }
    }

    public async Task<IResults> TableAddOrUpdate(TableAddOrUpdateDTO model)
    {
   
     
       
        if (model.Id == 0)
        {
            
            if (model.TableNumber == 0)
            {
                var tableList=  await _tableService.Where(x=>x.TableAreaId==model.TableAreaId&&!x.IsDeleted).AsNoTracking().ToListAsync();
                model.TableNumber = tableList.Count + 1;
            }
            var response = _mapper.Map<Table>(model);
            await _tableService.AddAsync(response);
            return new SuccessResult();
        }
        else
        {
            
            if (model.TableNumber == 0)
            {
                var tableList= await  _tableService.Where(x=>x.TableAreaId==model.TableAreaId&&!x.IsDeleted).AsNoTracking().ToListAsync();
                model.TableNumber = tableList.Count ;
            }
            var response = _mapper.Map<Table>(model);
            await _tableService.UpdateAsync(response);
            return new SuccessResult();
        }
     
        
    
    }

    public async Task<IResults> TableAreaDelete(long id)
    {
        var tableArea = await _tableAreaService.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (tableArea != null)
        {
            tableArea.IsDeleted = true;
            await _tableAreaService.UpdateAsync(tableArea);
            return new SuccessResult();
        }
        else
        {
            return new ErrorResult("Table Area Not Found");
        }
    }

    public async Task<IResults> TableDelete(long id)
    {
        var table = await _tableService.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (table != null)
        {
            table.IsDeleted = true;
            await _tableService.UpdateAsync(table);
            return new SuccessResult();
        }
        else
        {
            return new ErrorResult("Table Not Found");
        }
    }

    public async Task<IDataResults<ICollection<TableAreaListDTO>>> TableAreaListByCompanyId()
    {
        var companyId = GetLoggedCompanyId();
        var tableAreas = await _tableAreaService.Where(x => x.RestoCompanyId == companyId && !x.IsDeleted).ToListAsync();
        var response = _mapper.Map<ICollection<TableAreaListDTO>>(tableAreas);
        return new SuccessDataResult<ICollection<TableAreaListDTO>>(response);
    }

    public async Task<IDataResults<ICollection<TableListDTO>>> TableListTableAreaId(long tableAreaId)
    {
        var table = await _tableService.Where(x => x.TableAreaId == tableAreaId && !x.IsDeleted).ToListAsync();
        var response = _mapper.Map<ICollection<TableListDTO>>(table);
        return new SuccessDataResult<ICollection<TableListDTO>>(response);
    }

    public async Task<IDataResults<ICollection<TableListDTO>>> AllTableList()
    {
        List<Table> tableList = new List<Table>();
        var companyId = GetLoggedCompanyId();
        var tableArea = await _tableAreaService.Where(x => x.RestoCompanyId == companyId).ToListAsync();

        foreach (var item in tableArea)
        {
            var tableItemsTemp = await _tableService.Where(x => x.TableAreaId == item.Id && !x.IsDeleted).ToListAsync();

            tableList.AddRange(tableItemsTemp);
        }

        var response = _mapper.Map<ICollection<TableListDTO>>(tableList);
        return new SuccessDataResult<ICollection<TableListDTO>>(response);
    }
    
    public async Task<ICollection<TableListDTO>> AllTableListForICollection()
    {
        List<Table> tableList = new List<Table>();
        var companyId = GetLoggedCompanyId();
        var tableArea = await _tableAreaService.Where(x => x.RestoCompanyId == companyId).ToListAsync();

        foreach (var item in tableArea)
        {
            var tableItemsTemp = await _tableService.Where(x => x.TableAreaId == item.Id && !x.IsDeleted).ToListAsync();

            tableList.AddRange(tableItemsTemp);
        }

        var response = _mapper.Map<ICollection<TableListDTO>>(tableList);
        return response;
    }
    
    public long? GetLoggedCompanyId()
    {
        var userRoleString = _loggedUser.FindFirstValue("companyId");
        if (long.TryParse(userRoleString, out var userId))
        {
            return userId;
        }

        return null;
    }
    
    
}