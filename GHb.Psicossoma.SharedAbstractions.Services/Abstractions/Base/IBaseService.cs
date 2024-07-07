using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.SharedAbstractions.Services.Abstractions.Base;

public interface IBaseService<Dto> : IDisposable where Dto : BaseDto
{
    #region " Synchronous Operations "

    bool EntityExists(string entityName);

    void DropEntity(string entityName);

    List<string> GetEntityNames();

    ResultDto<Dto> Get(string id);

    ResultDto<Dto> Get(List<object> filters);

    ResultDto<Dto> Get(List<object> filters, int page, int pageSize, string? sortField = null, string? sortOrder = null);

    ResultDto<Dto> Get(List<object> filters, int page, int pageSize, bool basicFilter, bool caseInsentive = false);

    ResultDto<Dto> GetAll();

    bool Exists(Dictionary<string, object> filterDefinition);

    ResultDto<Dto> Insert(Dto dto);

    ResultDto<Dto> Update(Dto dto);

    ResultDto<Dto> Update(string id, Dto model);

    void Remove(string id);

    void Remove(List<string> selectedIds);

    #endregion

    #region " Asynchronous Operations "

    Task<ResultDto<Dto>> GetAsync(string id);

    Task<ResultDto<Dto>> GetAsync(List<object> filters, string? sortField, string? sortOrder);

    Task<ResultDto<Dto>> GetAsync(List<object> filters, bool basicFilter, bool caseInsentive = false);

    Task<ResultDto<Dto>> GetAllAsync();

    Task<ResultDto<Dto>> InsertAsync(Dto dto);

    Task<ResultDto<Dto>> InsertManyAsync(IEnumerable<Dto> dtos);

    Task<ResultDto<Dto>> UpdateAsync(Dto dto);

    Task<ResultDto<Dto>> UpdateAsync(string id, Dto model);

    Task<ResultDto<Dto>> UpdateAsync(List<object> filters, Dictionary<string, object?> updateValues);

    Task DeleteAsync(Dto dto);

    #endregion

}
