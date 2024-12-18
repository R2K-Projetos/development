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
    public class AgendaProfissionalService : BaseService<AgendaProfissionalDto, AgendaProfissional>, IAgendaProfissionalService
    {
        private readonly IAgendaProfissionalRepository _agendaProfissionalRepository;
        private readonly ILogger<AgendaProfissionalService> _logger;

        public AgendaProfissionalService(IAgendaProfissionalRepository agendaprofissionalRepository,
                                         ILogger<AgendaProfissionalService> logger,
                                         IMapper mapper) : base(agendaprofissionalRepository, mapper)
        {
            _agendaProfissionalRepository = agendaprofissionalRepository;
            _logger = logger;
        }

        public ResultDto<AgendaProfissionalDto> GetByProfissional(string ProfissionalId)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<AgendaProfissionalDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id
                                 ,ProfissionalId
                                 ,EspecialidadeId
                                 ,DiaSemana
                                 ,Hora
                                 ,Ativo
                                 FROM agendaprofissional
                                 WHERE ProfissionalId = {ProfissionalId};";

                DataTable result = _agendaProfissionalRepository.GetAll(selectQuery);
                List<AgendaProfissional> list = result.CreateListFromTable<AgendaProfissional>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<AgendaProfissional>, IEnumerable<AgendaProfissionalDto>>(list ?? Enumerable.Empty<AgendaProfissional>());
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

        public override ResultDto<AgendaProfissionalDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<AgendaProfissionalDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id
                                 ,ProfissionalId
                                 ,EspecialidadeId
                                 ,DiaSemana
                                 ,Hora
                                 ,Ativo
                                 FROM agendaprofissional
                                 WHERE Id = {id};";

                DataTable result = _agendaProfissionalRepository.Get(selectQuery);
                List<AgendaProfissional> item = result.CreateListFromTable<AgendaProfissional>();

                if (item?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = item.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<AgendaProfissional>, IEnumerable<AgendaProfissionalDto>>(item ?? Enumerable.Empty<AgendaProfissional>());
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

        public override ResultDto<AgendaProfissionalDto> Insert(AgendaProfissionalDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<AgendaProfissionalDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<AgendaProfissionalDto, AgendaProfissional>(dto);
                insertQuery = $@"INSERT INTO agendaprofissional 
                                 (ProfissionalId
                                 ,EspecialidadeId
                                 ,DiaSemana
                                 ,Hora
                                 ,Ativo)
                                 VALUES 
                                 ({entidade.ProfissionalId}
                                 ,{entidade.EspecialidadeId}
                                 ,'{entidade.DiaSemana}'
                                 ,'{entidade.Hora}'
                                 ,true);";

                long newId = _agendaProfissionalRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<AgendaProfissional, AgendaProfissionalDto>(entidade);

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

        public override ResultDto<AgendaProfissionalDto> Update(AgendaProfissionalDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<AgendaProfissionalDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<AgendaProfissionalDto, AgendaProfissional>(dto);
                updateQuery = $@"UPDATE agendaprofissional 
                                 SET ProfissionalId = {entidade.ProfissionalId}
                                 ,EspecialidadeId = {entidade.EspecialidadeId}
                                 ,DiaSemana = '{entidade.DiaSemana}'
                                 ,Hora = '{entidade.Hora}'
                                 ,Ativo = {entidade.Ativo}
                                 WHERE Id = {entidade.Id};";

                _agendaProfissionalRepository.Update(updateQuery);
                var item = _mapper.Map<AgendaProfissional, AgendaProfissionalDto>(entidade);

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
