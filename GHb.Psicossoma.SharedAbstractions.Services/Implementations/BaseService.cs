using AutoMapper;
using System.Diagnostics;
using Ghb.Psicossoma.Library.Extensions;
using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;
using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;
using Ghb.Psicossoma.SharedAbstractions.Domains.Abstractions.Base;
using Ghb.Psicossoma.SharedAbstractions.Services.Abstractions.Base;

namespace Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

public class BaseService<Dto, TDocument> : IBaseService<Dto> where Dto : BaseDto where TDocument : BaseEntity
{
    protected readonly IMapper _mapper;
    protected readonly string _entityName;
    private readonly IBaseRepository<TDocument> _repository;

    public BaseService(IBaseRepository<TDocument> repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
        _entityName = typeof(TDocument).Name;
    }

    #region "  Synchronous Operations  "

    public virtual bool EntityExists(string entityName)
    {
        return _repository.EntityExists(entityName);
    }
    public virtual ResultDto<Dto> Deactivate(string id)
    {
        throw new NotImplementedException();
    }

    public virtual void DropEntity(string entityName)
    {
        _repository.DropEntity(entityName);
    }

    public virtual List<string> GetEntityNames()
    {
        return _repository.GetEntityNames();
    }

    public virtual ResultDto<Dto> Get(string id)
    {
        throw new NotImplementedException();
    }

    public virtual ResultDto<Dto> Get(List<object> filters)
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        var returnValue = new ResultDto<Dto>();

        try
        {
            var result = _repository.Get(filters);

            if (result is null)
            {
                returnValue.BindError(404, $"{_entityName.ToUpper()}_EMPTY" ?? "Não foram encontrados dados para exibição");
            }
            else
            {
                returnValue.Items = _mapper.Map<IEnumerable<TDocument>, IEnumerable<Dto>>(result);
                returnValue.WasExecuted = true;
                returnValue.ResponseCode = 200;
            }
        }
        catch (FormatException fEx)
        {
            returnValue.BindError(415, fEx.GetErrorMessage());
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;

        return returnValue;
    }

    public virtual ResultDto<Dto> Get(List<object> filters, int page, int pageSize, string? sortField = null, string? sortOrder = null)
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        var returnValue = new ResultDto<Dto>();

        Dictionary<string, string> sortDefinition = new();

        if (!string.IsNullOrWhiteSpace(sortField) && !string.IsNullOrWhiteSpace(sortOrder))
            sortDefinition.Add(sortField, sortOrder);

        try
        {
            var result = _repository.Get(filters, page, pageSize, sortDefinition);

            if (result is null)
            {
                returnValue.BindError(404, $"{_entityName.ToUpper()}_EMPTY" ?? "Não foram encontrados dados para exibição");
            }
            else
            {
                returnValue.Items = _mapper.Map<IEnumerable<TDocument>, IEnumerable<Dto>>(result);
                returnValue.WasExecuted = true;
                returnValue.ResponseCode = 200;
                returnValue.PageSize = pageSize;
                returnValue.CurrentPage = page;
                returnValue.TotalItems = _repository.TotalItems(filters);
            }
        }
        catch (FormatException fEx)
        {
            returnValue.BindError(415, fEx.GetErrorMessage());
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;

        return returnValue;
    }

    public ResultDto<Dto> Get(List<object> filters, int page, int pageSize, bool basicFilter, bool caseInsentive = false)
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        var returnValue = new ResultDto<Dto>();

        try
        {
            var result = _repository.Get(filters, page, pageSize, basicFilter, caseInsentive);

            if (result is null)
            {
                returnValue.BindError(404, $"{_entityName.ToUpper()}_EMPTY" ?? "Não foram encontrados dados para exibição");
            }
            else
            {
                returnValue.Items = _mapper.Map<IEnumerable<TDocument>, IEnumerable<Dto>>(result);
                returnValue.WasExecuted = true;
                returnValue.ResponseCode = 200;
                returnValue.PageSize = pageSize;
                returnValue.CurrentPage = page;
                returnValue.TotalItems = _repository.TotalItems(filters);
            }
        }
        catch (FormatException fEx)
        {
            returnValue.BindError(415, fEx.GetErrorMessage());
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;

        return returnValue;
    }

    public virtual ResultDto<Dto> GetAll()
    {
        throw new NotImplementedException();
    }

    public virtual bool Exists(Dictionary<string, object> filterDefinition)
    {
        return _repository.Exists(filterDefinition);
    }

    public virtual ResultDto<Dto> Insert(Dto dto)
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        var returnValue = new ResultDto<Dto>();

        try
        {
            var result = _repository.Create(_mapper.Map<Dto, TDocument>(dto));
            var item = _mapper.Map<TDocument, Dto>(result);

            returnValue.Items = returnValue.Items.Concat(new[] { item });
            returnValue.WasExecuted = true;
            returnValue.ResponseCode = 200;
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;

        return returnValue;
    }

    public virtual ResultDto<Dto> InsertMany(IEnumerable<Dto> dtos)
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        var returnValue = new ResultDto<Dto>();

        try
        {
            _repository.CreateMany(_mapper.Map<IEnumerable<Dto>, IEnumerable<TDocument>>(dtos));

            returnValue.WasExecuted = true;
            returnValue.ResponseCode = 200;
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;

        return returnValue;
    }

    public virtual ResultDto<Dto> Update(Dto dto)
    {
        throw new NotImplementedException();
    }

    public virtual ResultDto<Dto> Update(string query)
    {
        throw new NotImplementedException();
    }

    public virtual ResultDto<Dto> Update(string id, Dto dto)
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        var returnValue = new ResultDto<Dto>();

        try
        {
            var updatedEntity = _mapper.Map<Dto, TDocument>(dto);

            _repository.Update(id, updatedEntity);

            returnValue.Items = returnValue.Items.Concat(new[] { dto });
            returnValue.WasExecuted = true;
            returnValue.ResponseCode = 200;
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;

        return returnValue;
    }

    public virtual ResultDto<Dto> Update(List<object> filters, Dictionary<string, object?> updateValues)
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        var returnValue = new ResultDto<Dto>();

        try
        {
            _repository.Update(filters, updateValues);

            returnValue.WasExecuted = true;
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;

        return returnValue;
    }

    public virtual void Remove(string id)
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        var returnValue = new ResultDto<Dto>();

        try
        {
            _repository.Remove(id);

            returnValue.WasExecuted = true;
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;
    }

    public virtual void Remove(List<string> selectedIds)
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        var returnValue = new ResultDto<Dto>();

        try
        {
            _repository.Remove(selectedIds);

            returnValue.WasExecuted = true;
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;
    }

    #endregion

    #region " Asynchronous Operations "

    public virtual async Task<ResultDto<Dto>> GetAsync(string id)
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        var returnValue = new ResultDto<Dto>();

        try
        {
            var result = await _repository.GetAsync(id);
            var item = _mapper.Map<TDocument, Dto>(result);

            returnValue.WasExecuted = true;
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;

        return returnValue;
    }

    public virtual Task<ResultDto<Dto>> GetAsync(List<object> filters, string? sortField, string? sortOrder)
    {
        throw new NotImplementedException();
    }

    public virtual Task<ResultDto<Dto>> GetAsync(List<object> filters, bool basicFilter, bool caseInsentive = false)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<ResultDto<Dto>> GetAllAsync()
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        var returnValue = new ResultDto<Dto>();

        try
        {
            var result = await _repository.GetAllAsync();

            returnValue.Items = _mapper.Map<IEnumerable<TDocument>, IEnumerable<Dto>>(result);
            returnValue.WasExecuted = true;
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;

        return returnValue;
    }

    public async virtual Task<ResultDto<Dto>> InsertAsync(Dto dto)
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        var returnValue = new ResultDto<Dto>();

        try
        {
            var result = await _repository.CreateAsync(_mapper.Map<Dto, TDocument>(dto));
            var item = _mapper.Map<TDocument, Dto>(result);

            returnValue.Items = returnValue.Items.Concat(new[] { item });
            returnValue.WasExecuted = true;
            returnValue.ResponseCode = 200;
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;

        return returnValue;
    }

    public async virtual Task<ResultDto<Dto>> InsertManyAsync(IEnumerable<Dto> dtos)
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        var returnValue = new ResultDto<Dto>();

        try
        {
            await _repository.CreateManyAsync(_mapper.Map<IEnumerable<Dto>, IEnumerable<TDocument>>(dtos));

            returnValue.WasExecuted = true;
            returnValue.ResponseCode = 200;
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;

        return returnValue;
    }

    public virtual Task<ResultDto<Dto>> UpdateAsync(Dto dto)
    {
        throw new NotImplementedException();
    }

    public async virtual Task<ResultDto<Dto>> UpdateAsync(string id, Dto document)
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        var returnValue = new ResultDto<Dto>();

        try
        {
            var updatedEntity = _mapper.Map<Dto, TDocument>(document);

            await _repository.UpdateAsync(id, updatedEntity);
            var item = _mapper.Map<TDocument, Dto>(updatedEntity);

            returnValue.Items = returnValue.Items.Concat(new[] { item });
            returnValue.WasExecuted = true;
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;

        return returnValue;
    }

    public async virtual Task<ResultDto<Dto>> UpdateAsync(List<object> filters, Dictionary<string, object?> updateValues)
    {
        Stopwatch elapsedTime = new();
        elapsedTime.Start();

        var returnValue = new ResultDto<Dto>();

        try
        {
            await _repository.UpdateAsync(filters, updateValues);

            returnValue.WasExecuted = true;
        }
        catch (Exception ex)
        {
            returnValue.BindError(500, ex.GetErrorMessage());
        }

        elapsedTime.Stop();
        returnValue.ElapsedTime = elapsedTime.Elapsed;

        return returnValue;
    }
    public virtual Task DeleteAsync(Dto dto)
    {
        throw new NotImplementedException();
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

            _repository.Dispose();

            disposedValue = true;
        }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    ~BaseService()
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
