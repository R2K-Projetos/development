using AutoMapper;
using Ghb.Psicossoma.Services.Dtos;
using Microsoft.Extensions.Logging;
using Ghb.Psicossoma.Domains.Entities;
using Microsoft.Extensions.Configuration;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;
using Ghb.Psicossoma.Library.Extensions;
using Ghb.Psicossoma.Repositories.Implementations;
using Serilog.Context;
using System.Data;
using System.Diagnostics;

namespace Ghb.Psicossoma.Services.Implementations
{
    public class CidService : BaseService<CidDto, Cid>, ICidService
    {
        private readonly ICidRepository _cidRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CidService> _logger;

        public CidService(ICidRepository cidRepository,
                          ILogger<CidService> logger,
                          IMapper mapper,
                          IConfiguration configuration) : base(cidRepository, mapper)
        {
            _cidRepository = cidRepository;
            _configuration = configuration;
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
                selectQuery = $@"SELECT Id, Codigo, Descricao FROM cid;";

                DataTable result = _cidRepository.GetAll(selectQuery);
                List<Cid> cids = result.CreateListFromTable<Cid>();

                if (cids?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = cids.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Cid>, IEnumerable<CidDto>>(cids ?? Enumerable.Empty<Cid>());
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
                selectQuery = $@"SELECT Id, Codigo, Descricao
                                 FROM cid
                                 WHERE id = {id};";

                DataTable result = _cidRepository.Get(selectQuery);
                List<Cid> cids = result.CreateListFromTable<Cid>();

                if (cids?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = cids.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Cid>, IEnumerable<CidDto>>(cids ?? Enumerable.Empty<Cid>());
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
                List<Cid> cids = result.CreateListFromTable<Cid>();

                if (cids?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = cids.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Cid>, IEnumerable<CidDto>>(cids ?? Enumerable.Empty<Cid>());
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
    }
}
