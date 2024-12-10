using AutoMapper;
using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Library.Extensions;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Data;
using System.Diagnostics;

namespace Ghb.Psicossoma.Services.Implementations
{
    public class CidService : BaseService<CidDto, Cid>, ICidService
    {
        private readonly ICidRepository _cidRepository;
        private readonly ILogger<CidService> _logger;

        public CidService(ICidRepository cidRepository,
                          ILogger<CidService> logger,
                          IMapper mapper) : base(cidRepository, mapper)
        {
            _cidRepository = cidRepository;
            _logger = logger;
        }

        public override ResultDto<CidDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<CidDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, Codigo, Nome FROM cid;";

                DataTable result = _cidRepository.GetAll(selectQuery);
                List<Cid> list = result.CreateListFromTable<Cid>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Cid>, IEnumerable<CidDto>>(list ?? Enumerable.Empty<Cid>());
                    returnValue.WasExecuted = true;
                    returnValue.ResponseCode = 200;
                }
                else
                {
                    returnValue.BindError(404, "Não foram encontrados dados para exibição");
                }
            }
            catch (Exception ex)
            {
                returnValue.BindError(500, ex.GetErrorMessage());
                LogContext.PushProperty("Query", selectQuery);
                _logger.LogError(ex, "Erro na recuperação dos dados");
            }

            elapsedTime.Stop();
            returnValue.ElapsedTime = elapsedTime.Elapsed;

            return returnValue;
        }

        public override ResultDto<CidDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<CidDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, Codigo, Nome
                                 FROM cid
                                 WHERE id = {id};";

                DataTable result = _cidRepository.Get(selectQuery);
                List<Cid> item = result.CreateListFromTable<Cid>();

                if (item?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = item.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Cid>, IEnumerable<CidDto>>(item ?? Enumerable.Empty<Cid>());
                    returnValue.WasExecuted = true;
                    returnValue.ResponseCode = 200;
                }
                else
                {
                    returnValue.BindError(404, "Não foram encontrados dados para exibição");
                }
            }
            catch (Exception ex)
            {
                returnValue.BindError(500, ex.GetErrorMessage());
                LogContext.PushProperty("Query", selectQuery);
                _logger.LogError(ex, "Erro na recuperação dos dados");
            }

            elapsedTime.Stop();
            returnValue.ElapsedTime = elapsedTime.Elapsed;

            return returnValue;
        }

        public ResultDto<CidDto> GetByCode(string code)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<CidDto> returnValue = new();

            try
            {
                DataTable result = _cidRepository.GetByCode(code);
                List<Cid> list = result.CreateListFromTable<Cid>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Cid>, IEnumerable<CidDto>>(list ?? Enumerable.Empty<Cid>());
                    returnValue.WasExecuted = true;
                    returnValue.ResponseCode = 200;
                }
                else
                {
                    returnValue.BindError(404, "Não foram encontrados dados para exibição");
                }
            }
            catch (Exception ex)
            {
                returnValue.BindError(500, ex.GetErrorMessage());
                _logger.LogError(ex, "Erro na recuperação dos dados");
            }

            elapsedTime.Stop();
            returnValue.ElapsedTime = elapsedTime.Elapsed;

            return returnValue;
        }

        public override ResultDto<CidDto> Insert(CidDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<CidDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<CidDto, Cid>(dto);
                insertQuery = $@"INSERT INTO cid 
                                 (Codigo, Nome)
                                 VALUES 
                                 ('{entidade.Codigo}', '{entidade.Nome}');";

                long newId = _cidRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<Cid, CidDto>(entidade);

                returnValue.Items = returnValue.Items.Concat(new[] { item });
                returnValue.WasExecuted = true;
                returnValue.ResponseCode = 200;
            }
            catch (Exception ex)
            {
                returnValue.BindError(500, ex.GetErrorMessage());
                LogContext.PushProperty("Query", insertQuery);
                _logger.LogError(ex, "Erro na gravação dos dados");
            }

            elapsedTime.Stop();
            returnValue.ElapsedTime = elapsedTime.Elapsed;

            return returnValue;
        }

        public override ResultDto<CidDto> Update(CidDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<CidDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<CidDto, Cid>(dto);
                updateQuery = $@"UPDATE cid 
                                 SET Codigo = '{entidade.Codigo}', Nome = '{entidade.Nome}'
                                 WHERE id = {entidade.Id};";

                _cidRepository.Update(updateQuery);
                var item = _mapper.Map<Cid, CidDto>(entidade);

                returnValue.Items = returnValue.Items.Concat(new[] { item });
                returnValue.WasExecuted = true;
                returnValue.ResponseCode = 200;
            }
            catch (Exception ex)
            {
                returnValue.BindError(500, ex.GetErrorMessage());
                LogContext.PushProperty("Query", updateQuery);
                _logger.LogError(ex, "Erro na gravação dos dados");
            }

            elapsedTime.Stop();
            returnValue.ElapsedTime = elapsedTime.Elapsed;

            return returnValue;
        }
    }
}
