using System.Data;
using MySql.Data.MySqlClient;
using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;
using Ghb.Psicossoma.SharedAbstractions.Domains.Abstractions.Base;
using Ghb.Psicossoma.SharedAbstractions.Repositories.Abstractions;

namespace Ghb.Psicossoma.SharedAbstractions.Repositories.Implementations.Base;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly IContextDatabaseSettings _settings;

    public BaseRepository(IContextDatabaseSettings settings)
    {
        _settings = settings;
    }

    public virtual TEntity Create(TEntity model)
    {
        throw new NotImplementedException();
    }

    public K Create<K>(object _entity, K model)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> CreateAsync(TEntity model)
    {
        throw new NotImplementedException();
    }

    public void CreateMany(IEnumerable<TEntity> documents)
    {
        throw new NotImplementedException();
    }

    public Task CreateManyAsync(IEnumerable<TEntity> models)
    {
        throw new NotImplementedException();
    }

    public void DropEntity(string entityName)
    {
        throw new NotImplementedException();
    }

    public Task DropEntityAsync(string entityName)
    {
        throw new NotImplementedException();
    }

    public bool EntityExists(string entityName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> EntityExistsAsync(string entityName)
    {
        throw new NotImplementedException();
    }

    public bool Exists(Dictionary<string, object> filterDefinition)
    {
        throw new NotImplementedException();
    }

    public List<TEntity> Get(List<object> filters, bool basicFilter = false)
    {
        throw new NotImplementedException();
    }

    public List<TEntity> Get(List<object> filters, int page, int pageSize, Dictionary<string, string> sorting)
    {
        throw new NotImplementedException();
    }

    public List<TEntity> Get(List<object> filters, Dictionary<string, string> sorting)
    {
        throw new NotImplementedException();
    }

    public List<TEntity> Get(List<object> filters, int page, int pageSize, bool basicFilter, bool caseInsentive = false)
    {
        throw new NotImplementedException();
    }

    public List<TEntity> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<List<TEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public object GetEntity<K>(string entityName)
    {
        throw new NotImplementedException();
    }

    public List<string> GetEntityNames()
    {
        throw new NotImplementedException();
    }

    public Task<List<string>> GetEntityNamesAsync()
    {
        throw new NotImplementedException();
    }

    public void Remove(string id)
    {
        throw new NotImplementedException();
    }

    public long Remove(List<string> selectedIds)
    {
        throw new NotImplementedException();
    }

    public long RemoveAll()
    {
        throw new NotImplementedException();
    }

    public Task<long> RemoveAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<long> RemoveAsync(List<string> selectedIds)
    {
        throw new NotImplementedException();
    }

    public long TotalItems(List<object> filters)
    {
        throw new NotImplementedException();
    }

    public void Update(string id, TEntity model)
    {
        throw new NotImplementedException();
    }

    public void Update(List<object> filters, Dictionary<string, object?> updateValues)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(string id, TEntity model)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(List<object> filters, Dictionary<string, object?> updateValues)
    {
        throw new NotImplementedException();
    }

    #region " ADO.Net operações "

    public DataTable Get(string filterQuery)
    {
        MySqlConnection? cn = null;
        DataSet selectionData = new();

        try
        {
            using (cn = new(_settings.ConnectionString))
            {
                cn.Open();

                using (MySqlCommand cmd = new(filterQuery, cn))
                {
                    MySqlDataAdapter store = new(cmd);
                    store.Fill(selectionData);
                }

                return selectionData.Tables[0];
            }
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            cn.Close();
        }
    }

    public DataTable GetAll(string selectQuery)
    {
        MySqlConnection? cn = null;
        DataSet selectionData = new();

        try
        {
            using (cn = new(_settings.ConnectionString))
            {
                cn.Open();

                using (MySqlCommand cmd = new(selectQuery, cn))
                {
                    MySqlDataAdapter store = new(cmd);
                    store.Fill(selectionData);
                }

                return selectionData.Tables[0];
            }
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            cn.Close();
        }
    }

    public long Insert(string insertQuery)
    {
        long newId = 0;
        MySqlConnection? cn = null;

        try
        {
            using (cn = new(_settings.ConnectionString))
            {
                cn.Open();

                using MySqlCommand cmd = new(insertQuery, cn);
                cmd.ExecuteNonQuery();
                newId = cmd.LastInsertedId;
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            cn.Close();
        }

        return newId;
    }

    #endregion

    #region "  IDisposable Support  "

    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            //Context.Dispose();

            disposedValue = true;
        }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    ~BaseRepository()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(false);
    }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);
        // TODO: uncomment the following line if the finalizer is overridden above.
        GC.SuppressFinalize(this);
    }

    #endregion
}
