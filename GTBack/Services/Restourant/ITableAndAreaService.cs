using GTBack.Core.DTO.Restourant.Request;
using GTBack.Core.DTO.Restourant.Response;
using GTBack.Core.Results;

namespace GTBack.Core.Services.Restourant;

public interface ITableAndAreaService
{
    Task<IResults> TableAreaAddOrUpdate(TableAreaAddOrUpdateDTO model);
    Task<IResults> TableAddOrUpdate(TableAddOrUpdateDTO model);
    Task<IResults> TableAreaDelete(long id);
    Task<IResults> TableDelete(long id);
    Task<IDataResults<ICollection<TableAreaListDTO>>> TableAreaListByCompanyId();
    Task<IDataResults<ICollection<TableListDTO>>> TableListTableAreaId(long tableAreaId);
    Task<IDataResults<ICollection<TableListDTO>>> AllTableList();
    Task<ICollection<TableListDTO>> AllTableListForICollection();
}