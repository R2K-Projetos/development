using System.Data;

namespace Ghb.Psicossoma.SharedAbstractions.Domains.Abstractions.Base;

public interface IBaseRepository<TDocument> : IDisposable where TDocument : IBaseEntity
{
    #region " Synchronous Operations "

    TDocument Create(TDocument model);

    K Create<K>(object _entity, K model);

    void CreateMany(IEnumerable<TDocument> documents);

    void DropEntity(string entityName);

    List<string> GetEntityNames();

    bool EntityExists(string entityName);

    bool Exists(Dictionary<string, object> filterDefinition);

    object GetEntity<K>(string entityName);

    DataTable GetAll(string selectQuery);

    DataTable Get(string filterQuery);

    List<TDocument> Get(List<object> filters, bool basicFilter = false);

    List<TDocument> Get(List<object> filters, int page, int pageSize, Dictionary<string, string> sorting);

    List<TDocument> Get(List<object> filters, Dictionary<string, string> sorting);

    List<TDocument> Get(List<object> filters, int page, int pageSize, bool basicFilter, bool caseInsentive = false);

    void Remove(string id);

    long Remove(List<string> selectedIds);

    long RemoveAll();

    void Update(string id, TDocument model);

    void Update(List<object> filters, Dictionary<string, object?> updateValues);

    long TotalItems(List<object> filters);

    #endregion

    #region " Asynchronous Operations "

    Task<TDocument> CreateAsync(TDocument model);

    Task CreateManyAsync(IEnumerable<TDocument> models);

    Task DropEntityAsync(string entityName);

    Task<List<string>> GetEntityNamesAsync();

    Task<bool> EntityExistsAsync(string entityName);

    Task<List<TDocument>> GetAllAsync();

    Task<TDocument> GetAsync(string id);

    Task RemoveAsync(string id);

    Task<long> RemoveAsync(List<string> selectedIds);

    Task<long> RemoveAllAsync();

    Task UpdateAsync(string id, TDocument model);

    Task UpdateAsync(List<object> filters, Dictionary<string, object?> updateValues);

    #endregion

    long Insert(string insertQuery);

}
