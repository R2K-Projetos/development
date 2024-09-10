using AutoMapper;
using System.Data;
using Serilog.Context;
using System.Diagnostics;
using Ghb.Psicossoma.Services.Dtos;
using Microsoft.Extensions.Logging;
using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Library.Extensions;
using Microsoft.Extensions.Configuration;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Services.Implementations
{
    public class ProdutoConvenioService : BaseService<ProdutoConvenioDto, ProdutoConvenio>, IProdutoConvenioService
    {
        private readonly IProdutoConvenioRepository _produtoConvenioRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProdutoConvenioService> _logger;

        public ProdutoConvenioService(IProdutoConvenioRepository produtoConvenioRepository,
                                      ILogger<ProdutoConvenioService> logger,
                                      IMapper mapper,
                                      IConfiguration configuration) : base(produtoConvenioRepository, mapper)
        {
            _produtoConvenioRepository = produtoConvenioRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<ProdutoConvenioDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProdutoConvenioDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, Descricao FROM produtoConvenio;";

                DataTable result = _produtoConvenioRepository.GetAll(selectQuery);
                List<ProdutoConvenio> produtos = result.CreateListFromTable<ProdutoConvenio>();

                if (produtos?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = produtos.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<ProdutoConvenio>, IEnumerable<ProdutoConvenioDto>>(produtos ?? Enumerable.Empty<ProdutoConvenio>());
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


        public ResultDto<ProdutoConvenioDto> GetByDescription(string content)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProdutoConvenioDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, Descricao
                                 FROM produtoConvenio
                                 WHERE Descricao LIKE '%{content}%'
                                 LIMIT 5;";

                DataTable result = _produtoConvenioRepository.Get(selectQuery);
                List<ProdutoConvenio> produtos = result.CreateListFromTable<ProdutoConvenio>();

                if (produtos?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = produtos.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<ProdutoConvenio>, IEnumerable<ProdutoConvenioDto>>(produtos ?? Enumerable.Empty<ProdutoConvenio>());
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
    }
}
