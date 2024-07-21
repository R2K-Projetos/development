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
    public class ConvenioService : BaseService<ConvenioDto, Convenio>, IConvenioService
    {
        private readonly IConvenioRepository _convenioRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ConvenioService> _logger;

        public ConvenioService(IConvenioRepository convenioRepository,
                               ILogger<ConvenioService> logger,
                               IMapper mapper,
                               IConfiguration configuration) : base(convenioRepository, mapper)
        {
            _convenioRepository = convenioRepository;
            _configuration = configuration;
            _logger = logger;
        }


        public override ResultDto<ConvenioDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ConvenioDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, PlanoSaudeId, PlanoConvenioId, ProdutoConvenioId, Identificacao, Acomodacao, Cns, Cobertura, Empresa, Ativo
                                 FROM convenio
                                 WHERE id = {id};";

                DataTable result = _convenioRepository.Get(selectQuery);
                List<Convenio> convenios = result.CreateListFromTable<Convenio>();

                if (convenios?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = convenios.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Convenio>, IEnumerable<ConvenioDto>>(convenios ?? Enumerable.Empty<Convenio>());
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

        public override ResultDto<ConvenioDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ConvenioDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, PlanoSaudeId, PlanoConvenioId, ProdutoConvenioId, Identificacao, Acomodacao, Cns, Cobertura, Empresa, Ativo
                                 FROM convenio;";

                DataTable result = _convenioRepository.GetAll(selectQuery);
                List<Convenio> convenios = result.CreateListFromTable<Convenio>();

                if (convenios?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = convenios.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Convenio>, IEnumerable<ConvenioDto>>(convenios ?? Enumerable.Empty<Convenio>());
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
