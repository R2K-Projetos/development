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
    public class ProntuarioHistoricoService : BaseService<ProntuarioHistoricoDto, ProntuarioHistorico>, IProntuarioHistoricoService
    {
        private readonly IProntuarioHistoricoRepository _prontuarioHistoricoRepository;
        private readonly ILogger<ProntuarioHistoricoService> _logger;

        public ProntuarioHistoricoService(IProntuarioHistoricoRepository prontuarioHistoricoRepository,
                                          ILogger<ProntuarioHistoricoService> logger,
                                          IMapper mapper) : base(prontuarioHistoricoRepository, mapper)
        {
            _prontuarioHistoricoRepository = prontuarioHistoricoRepository;
            _logger = logger;
        }

        public override ResultDto<ProntuarioHistoricoDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProntuarioHistoricoDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id
                                        ,ProntuiarioId
                                        ,ProfissionalId
                                        ,DescricaoGeral
                                        ,DescricaoReservada
                                        ,DataHistorico
                                        ,Ativo
                                   FROM prontuarioHistorico;";

                DataTable result = _prontuarioHistoricoRepository.GetAll(selectQuery);
                List<ProntuarioHistorico> list = result.CreateListFromTable<ProntuarioHistorico>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<ProntuarioHistorico>, IEnumerable<ProntuarioHistoricoDto>>(list ?? Enumerable.Empty<ProntuarioHistorico>());
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

        public override ResultDto<ProntuarioHistoricoDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProntuarioHistoricoDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id
                                        ,ProntuiarioId
                                        ,ProfissionalId
                                        ,DescricaoGeral
                                        ,DescricaoReservada
                                        ,DataHistorico
                                        ,Ativo
                                   FROM prontuarioHistorico
                                  WHERE Id = {id};";

                DataTable result = _prontuarioHistoricoRepository.Get(selectQuery);
                List<ProntuarioHistorico> item = result.CreateListFromTable<ProntuarioHistorico>();

                if (item?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = item.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<ProntuarioHistorico>, IEnumerable<ProntuarioHistoricoDto>>(item ?? Enumerable.Empty<ProntuarioHistorico>());
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

        public override ResultDto<ProntuarioHistoricoDto> Insert(ProntuarioHistoricoDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProntuarioHistoricoDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<ProntuarioHistoricoDto, ProntuarioHistorico>(dto);
                insertQuery = $@"INSERT INTO prontuariohistorico 
                                 (ProntuarioId 
                                 ,ProfissionalId
                                 ,DescricaoGeral
                                 ,DescricaoReservada
                                 ,DataHistorico
                                 ,Ativo)
                                 VALUES 
                                 ({entidade.ProntuarioId}
                                 ,{entidade.ProfissionalId}
                                 ,'{entidade.DescricaoGeral}'
                                 ,'{entidade.DescricaoReservada}'
                                 ,'{entidade.DataHistorico:yyyy-MM-dd}'
                                 ,true);";

                long newId = _prontuarioHistoricoRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<ProntuarioHistorico, ProntuarioHistoricoDto>(entidade);

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

        public override ResultDto<ProntuarioHistoricoDto> Update(ProntuarioHistoricoDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProntuarioHistoricoDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<ProntuarioHistoricoDto, ProntuarioHistorico>(dto);
                updateQuery = $@"UPDATE prontuariohistorico 
                                 SET ProfissionalId = {entidade.ProfissionalId}
                                 ,DescricaoGeral = '{entidade.DescricaoGeral}'
                                 ,DescricaoReservada = '{entidade.DescricaoReservada}'
                                 ,DataHistorico = '{entidade.DataHistorico:yyyy-MM-dd}'
                                 ,Ativo = {entidade.Ativo}
                                 WHERE id = {entidade.Id};";

                _prontuarioHistoricoRepository.Update(updateQuery);
                var item = _mapper.Map<ProntuarioHistorico, ProntuarioHistoricoDto>(entidade);

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
