using AutoMapper;
using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Library.Extensions;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Data;
using System.Diagnostics;

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

        ResultDto<ConvenioResponseDto> IConvenioService.Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ConvenioResponseDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select c.Id
                                        ,c.PlanoSaudeId
                                        ,c.PlanoConvenioId
                                        ,c.ProdutoConvenioId
                                        ,c.Identificacao
                                        ,c.Acomodacao
                                        ,c.Cns
                                        ,c.Cobertura
                                        ,c.Empresa
                                        ,c.Ativo
                                        ,pls.Descricao as PlanoSaude
                                        ,plc.Descricao as PlanoConvenio
                                        ,pc.Descricao as ProdutoConvenio
                                   FROM convenio c
                                  inner join planosaude pls on pls.Id = c.PlanoSaudeId
                                   left join planoconvenio plc on plc.Id = c.PlanoConvenioId
                                   left join produtoconvenio pc on pc.Id = c.ProdutoConvenioId
                                  WHERE c.id = {id};";

                DataTable result = _convenioRepository.Get(selectQuery);
                List<ConvenioResponse> convenio = result.CreateListFromTable<ConvenioResponse>();

                if (convenio?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = convenio.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<ConvenioResponse>, IEnumerable<ConvenioResponseDto>>(convenio ?? Enumerable.Empty<ConvenioResponse>());
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

        ResultDto<ConvenioResponseDto> IConvenioService.GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ConvenioResponseDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select c.Id
                                        ,c.PlanoSaudeId
                                        ,c.PlanoConvenioId
                                        ,c.ProdutoConvenioId
                                        ,c.Identificacao
                                        ,c.Acomodacao
                                        ,c.Cns
                                        ,c.Cobertura
                                        ,c.Empresa
                                        ,c.Ativo
                                        ,pls.Descricao as PlanoSaude
                                        ,plc.Descricao as PlanoConvenio
                                        ,pc.Descricao as ProdutoConvenio
                                   FROM convenio c
                                  inner join planosaude pls on pls.Id = c.PlanoSaudeId
                                   left join planoconvenio plc on plc.Id = c.PlanoConvenioId
                                   left join produtoconvenio pc on pc.Id = c.ProdutoConvenioId
                                  where 1 = 1
                                  order by pls.Descricao
                                          ,plc.Descricao
                                          ,pc.Descricao";

                DataTable result = _convenioRepository.GetAll(selectQuery);
                List<ConvenioResponse> convenios = result.CreateListFromTable<ConvenioResponse>();

                if (convenios?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = convenios.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<ConvenioResponse>, IEnumerable<ConvenioResponseDto>>(convenios ?? Enumerable.Empty<ConvenioResponse>());
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
