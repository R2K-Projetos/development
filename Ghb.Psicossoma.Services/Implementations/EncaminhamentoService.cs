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
    public class EncaminhamentoService : BaseService<EncaminhamentoDto, Encaminhamento>, IEncaminhamentoService
    {
        private readonly IEncaminhamentoRepository _encaminhamentoRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EncaminhamentoService> _logger;

        public EncaminhamentoService(IEncaminhamentoRepository encaminhamentoRepository,
                                     ILogger<EncaminhamentoService> logger,
                                     IMapper mapper,
                                     IConfiguration configuration) : base(encaminhamentoRepository, mapper)
        {
            _encaminhamentoRepository = encaminhamentoRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<EncaminhamentoDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<EncaminhamentoDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, PacienteId, EspecialidadeId, ConvenioId, CidId, TotalSessoes, MaximoSessoes, QuantidadeSessoes, SolicitacaoMedica, Observacao, Ativo
                                 FROM encaminhamento
                                 WHERE id = {id};";

                DataTable result = _encaminhamentoRepository.Get(selectQuery);
                List<Encaminhamento> encaminhamentos = result.CreateListFromTable<Encaminhamento>();

                if (encaminhamentos?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = encaminhamentos.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Encaminhamento>, IEnumerable<EncaminhamentoDto>>(encaminhamentos ?? Enumerable.Empty<Encaminhamento>());
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

        public override ResultDto<EncaminhamentoDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<EncaminhamentoDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, PacienteId, EspecialidadeId, ConvenioId, CidId, TotalSessoes, MaximoSessoes, QuantidadeSessoes, SolicitacaoMedica, Observacao, Ativo
                                 FROM encaminhamento;";

                DataTable result = _encaminhamentoRepository.GetAll(selectQuery);
                List<Encaminhamento> encaminhamentos = result.CreateListFromTable<Encaminhamento>();

                if (encaminhamentos?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = encaminhamentos.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Encaminhamento>, IEnumerable<EncaminhamentoDto>>(encaminhamentos ?? Enumerable.Empty<Encaminhamento>());
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

        public ResultDto<EncaminhamentoDto> GetByIdPaciente(int id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<EncaminhamentoDto> returnValue = new();

            try
            {
                DataTable result = _encaminhamentoRepository.GetByIdPaciente(id);
                List<Encaminhamento> cids = result.CreateListFromTable<Encaminhamento>();

                if (cids?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = cids.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Encaminhamento>, IEnumerable<EncaminhamentoDto>>(cids ?? Enumerable.Empty<Encaminhamento>());
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

        public override ResultDto<EncaminhamentoDto> Insert(EncaminhamentoDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<EncaminhamentoDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var encaminhamento = _mapper.Map<EncaminhamentoDto, Encaminhamento>(dto);
                insertQuery = $@"INSERT INTO encaminhamento 
                                 (Id,  
                                 PacienteId,  
                                 EspecialidadeId,  
                                 ConvenioId,  
                                 CidId,  
                                 TotalSessoes,  
                                 MaximoSessoes,  
                                 QuantidadeSessoes,  
                                 SolicitacaoMedica,  
                                 Observacao,  
                                 Ativo)
                                 VALUES
                                 (null,  
                                 {encaminhamento.PacienteId},  
                                 {encaminhamento.EspecialidadeId},  
                                 {encaminhamento.PlanoSaudeId},  
                                 {encaminhamento.CidId},  
                                 {encaminhamento.TotalSessoes},  
                                 {encaminhamento.MaximoSessoes},  
                                 {encaminhamento.QuantidadeSessoes},  
                                 {encaminhamento.SolicitacaoMedica},  
                                 '{encaminhamento.Observacao}',  
                                 {encaminhamento.Ativo});";

                long newId = _encaminhamentoRepository.Insert(insertQuery);
                if (newId > 0)
                    encaminhamento.Id = (int)newId;

                var item = _mapper.Map<Encaminhamento, EncaminhamentoDto>(encaminhamento);

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
    }
}
