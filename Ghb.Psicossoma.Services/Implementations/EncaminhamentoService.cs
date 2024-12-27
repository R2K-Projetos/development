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
    public class EncaminhamentoService : BaseService<EncaminhamentoDto, Encaminhamento>, IEncaminhamentoService
    {
        private readonly IEncaminhamentoRepository _encaminhamentoRepository;
        private readonly ILogger<EncaminhamentoService> _logger;

        public EncaminhamentoService(IEncaminhamentoRepository encaminhamentoRepository,
                                     ILogger<EncaminhamentoService> logger,
                                     IMapper mapper) : base(encaminhamentoRepository, mapper)
        {
            _encaminhamentoRepository = encaminhamentoRepository;
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
                selectQuery = $@"SELECT Id
                                        ,PacienteId
                                        ,EspecialidadeId
                                        ,PlanoSaudeId
                                        ,CidId
                                        ,TotalSessoes
                                        ,MaximoSessoes
                                        ,SessoesRealizadas
                                        ,SolicitacaoMedica
                                        ,Observacao
                                        ,Ativo
                                        ,DataCadastro
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

        public ResultDto<EncaminhamentoResponseDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<EncaminhamentoResponseDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT e.Id
                                        ,e.PacienteId
                                        ,e.EspecialidadeId
                                        ,e.PlanoSaudeId
                                        ,e.CidId
                                        ,e.TotalSessoes
                                        ,e.MaximoSessoes
                                        ,e.SessoesRealizadas
                                        ,e.SolicitacaoMedica
                                        ,e.Observacao
                                        ,e.Ativo
                                        ,e.DataCadastro
                                        ,p.Nome as NomePaciente
                                        ,es.Nome as Especialidade
                                        ,pl.Nome as PlanoSaude
                                        ,cn.Nome as Convenio
                                        ,c.Codigo as CidCodigo
                                        ,c.Nome as CidDescricao
                                   FROM encaminhamento e
                                  INNER JOIN paciente pc ON pc.Id = e.PacienteId
                                  INNER JOIN pessoa p ON p.Id = pc.PessoaId
                                  INNER JOIN especialidade es ON es.Id = e.EspecialidadeId
                                  INNER JOIN planosaude pl ON pl.Id = e.PlanoSaudeId
                                  INNER JOIN convenio cn ON cn.Id = pl.ConvenioId
                                  INNER JOIN cid c ON c.Id = e.CidId
                                  ORDER BY p.Nome, e.DataCadastro DESC;";

                DataTable result = _encaminhamentoRepository.GetAll(selectQuery);
                List<EncaminhamentoResponse> encaminhamentos = result.CreateListFromTable<EncaminhamentoResponse>();

                if (encaminhamentos?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = encaminhamentos.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<EncaminhamentoResponse>, IEnumerable<EncaminhamentoResponseDto>>(encaminhamentos ?? Enumerable.Empty<EncaminhamentoResponse>());
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
                                 (Id
                                 ,PacienteId
                                 ,EspecialidadeId
                                 ,PlanoSaudeId
                                 ,CidId
                                 ,TotalSessoes
                                 ,MaximoSessoes
                                 ,SessoesRealizadas
                                 ,SolicitacaoMedica
                                 ,Observacao
                                 ,Ativo
                                 ,DataCadastro)
                                 VALUES
                                 (null
                                 ,{encaminhamento.PacienteId}
                                 ,{encaminhamento.EspecialidadeId}
                                 ,{encaminhamento.PlanoSaudeId}
                                 ,{encaminhamento.CidId}
                                 ,{encaminhamento.TotalSessoes}
                                 ,{encaminhamento.MaximoSessoes}
                                 ,0
                                 ,{encaminhamento.SolicitacaoMedica}
                                 ,'{encaminhamento.Observacao}'
                                 ,{encaminhamento.Ativo}'
                                 ,now());";

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

        public override ResultDto<EncaminhamentoDto> Update(EncaminhamentoDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<EncaminhamentoDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<EncaminhamentoDto, Encaminhamento>(dto);
                updateQuery = $@"UPDATE encaminhamento 
                                 SET EspecialidadeId = {entidade.EspecialidadeId}
                                 ,PlanoSaudeId = {entidade.PlanoSaudeId}
                                 ,CidId = {entidade.CidId}
                                 ,TotalSessoes = {entidade.TotalSessoes}
                                 ,MaximoSessoes = {entidade.MaximoSessoes}
                                 ,SolicitacaoMedica = {entidade.SolicitacaoMedica}
                                 ,Observacao = '{entidade.Observacao}'
                                 ,Ativo = {entidade.Ativo}
                                 WHERE id = {entidade.Id};";

                _encaminhamentoRepository.Update(updateQuery);
                var item = _mapper.Map<Encaminhamento, EncaminhamentoDto>(entidade);

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
