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
    public class ProntuarioHistoricoService : BaseService<ProntuarioHistoricoDto, ProntuarioHistorico>, IProntuarioHistoricoService
    {
        private readonly IProntuarioHistoricoRepository _prontuarioHistoricoRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProntuarioHistoricoService> _logger;

        public ProntuarioHistoricoService(IProntuarioHistoricoRepository prontuarioHistoricoRepository,
                                          ILogger<ProntuarioHistoricoService> logger,
                                          IMapper mapper,
                                          IConfiguration configuration) : base(prontuarioHistoricoRepository, mapper)
        {
            _prontuarioHistoricoRepository = prontuarioHistoricoRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<ProntuarioHistoricoDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProntuarioHistoricoDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, EncaminhamentoId, ProntuiarioId, ProfissionalId, DescricaoGeral, DescricaoReservada, Data, Ativo
                                 FROM prontuarioHistorico
                                 WHERE Id = {id};";

                DataTable result = _prontuarioHistoricoRepository.Get(selectQuery);
                List<ProntuarioHistorico> prontuarios = result.CreateListFromTable<ProntuarioHistorico>();

                if (prontuarios?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = prontuarios.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<ProntuarioHistorico>, IEnumerable<ProntuarioHistoricoDto>>(prontuarios ?? Enumerable.Empty<ProntuarioHistorico>());
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

        public override ResultDto<ProntuarioHistoricoDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProntuarioHistoricoDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, EncaminhamentoId, ProntuiarioId, ProfissionalId, DescricaoGeral, DescricaoReservada, Data, Ativo
                                 FROM prontuarioHistorico;";

                DataTable result = _prontuarioHistoricoRepository.GetAll(selectQuery);
                List<ProntuarioHistorico> prontuarios = result.CreateListFromTable<ProntuarioHistorico>();

                if (prontuarios?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = prontuarios.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<ProntuarioHistorico>, IEnumerable<ProntuarioHistoricoDto>>(prontuarios ?? Enumerable.Empty<ProntuarioHistorico>());
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
