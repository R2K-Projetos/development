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
    public class ConsultaService : BaseService<ConsultaDto, Consulta>, IConsultaService
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly ILogger<ConsultaService> _logger;

        public ConsultaService(IConsultaRepository consultaRepository,
                               ILogger<ConsultaService> logger,
                               IMapper mapper) : base(consultaRepository, mapper)
        {
            _consultaRepository = consultaRepository;
            _logger = logger;
        }

        public override ResultDto<ConsultaDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ConsultaDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id
                                 ,ProfissionalId
                                 ,EncaminhamentoId
                                 ,ProcedimentoDetalheId
                                 ,Valor
                                 ,DataConsulta
                                 ,Hora
                                 ,PacienteAvisado
                                 ,ProfissionalAvisado
                                 ,PacienteCompareceu
                                 ,ProfissionalCompareceu
                                 ,PacienteDesmarcou
                                 ,ProfissionalDesmarcou
                                 ,Ativo
                                 FROM consulta;";

                DataTable result = _consultaRepository.GetAll(selectQuery);
                List<Consulta> list = result.CreateListFromTable<Consulta>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Consulta>, IEnumerable<ConsultaDto>>(list ?? Enumerable.Empty<Consulta>());
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

        public override ResultDto<ConsultaDto> Get(string Id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ConsultaDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id
                                 ,ProfissionalId
                                 ,EncaminhamentoId
                                 ,ProcedimentoDetalheId
                                 ,Valor
                                 ,DataConsulta
                                 ,Hora
                                 ,PacienteAvisado
                                 ,ProfissionalAvisado
                                 ,PacienteCompareceu
                                 ,ProfissionalCompareceu
                                 ,PacienteDesmarcou
                                 ,ProfissionalDesmarcou
                                 ,Ativo
                                 FROM consulta
                                 WHERE Id = {Id};";

                DataTable result = _consultaRepository.Get(selectQuery);
                List<Consulta> item = result.CreateListFromTable<Consulta>();

                if (item?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = item.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Consulta>, IEnumerable<ConsultaDto>>(item ?? Enumerable.Empty<Consulta>());
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

        public override ResultDto<ConsultaDto> Insert(ConsultaDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ConsultaDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<ConsultaDto, Consulta>(dto);
                insertQuery = $@"INSERT INTO consulta 
                                 (ProfissionalId
                                 ,EncaminhamentoId
                                 ,ProcedimentoDetalheId
                                 ,Valor
                                 ,DataConsulta
                                 ,Hora
                                 ,PacienteAvisado
                                 ,ProfissionalAvisado
                                 ,PacienteCompareceu
                                 ,ProfissionalCompareceu
                                 ,PacienteDesmarcou
                                 ,ProfissionalDesmarcou
                                 ,Ativo)
                                 VALUES 
                                 ({entidade.ProfissionalId}
                                 ,{entidade.EncaminhamentoId}
                                 ,{entidade.ProcedimentoDetalheId}
                                 ,{entidade.Valor}
                                 ,'{entidade.DataConsulta}'
                                 ,'{entidade.Hora}'
                                 ,{entidade.PacienteAvisado}
                                 ,{entidade.ProfissionalAvisado}
                                 ,{entidade.PacienteCompareceu}
                                 ,{entidade.ProfissionalCompareceu}
                                 ,{entidade.PacienteDesmarcou}
                                 ,{entidade.ProfissionalDesmarcou}
                                 ,true);";

                long newId = _consultaRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<Consulta, ConsultaDto>(entidade);

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

        public override ResultDto<ConsultaDto> Update(ConsultaDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ConsultaDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<ConsultaDto, Consulta>(dto);
                updateQuery = $@"UPDATE consulta 
                                 SET ProfissionalId = {entidade.ProfissionalId}
                                 ,EncaminhamentoId = {entidade.EncaminhamentoId}
                                 ,ProcedimentoDetalheId = {entidade.ProcedimentoDetalheId}
                                 ,Valor = {entidade.Valor}
                                 ,DataConsulta = '{entidade.DataConsulta}'
                                 ,Hora = '{entidade.Hora}'
                                 ,PacienteAvisado = {entidade.PacienteAvisado}
                                 ,ProfissionalAvisado = {entidade.ProfissionalAvisado}
                                 ,PacienteCompareceu = {entidade.PacienteCompareceu}
                                 ,ProfissionalCompareceu = {entidade.ProfissionalCompareceu}
                                 ,PacienteDesmarcou = {entidade.PacienteDesmarcou}
                                 ,ProfissionalDesmarcou = {entidade.ProfissionalDesmarcou}
                                 ,Ativo = {entidade.Ativo}
                                 WHERE id = {entidade.Id};";

                _consultaRepository.Update(updateQuery);
                var item = _mapper.Map<Consulta, ConsultaDto>(entidade);

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
