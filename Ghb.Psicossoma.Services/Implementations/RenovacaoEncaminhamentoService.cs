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
    public class RenovacaoEncaminhamentoService : BaseService<RenovacaoEncaminhamentoDto, RenovacaoEncaminhamento>, IRenovacaoEncaminhamentoService
    {
        private readonly IRenovacaoEncaminhamentoRepository _renovacaoEncaminhamentoRepository;
        private readonly ILogger<RenovacaoEncaminhamentoService> _logger;

        public RenovacaoEncaminhamentoService(IRenovacaoEncaminhamentoRepository renovacaoEncaminhamentoRepository,
                               ILogger<RenovacaoEncaminhamentoService> logger,
                               IMapper mapper) : base(renovacaoEncaminhamentoRepository, mapper)
        {
            _renovacaoEncaminhamentoRepository = renovacaoEncaminhamentoRepository;
            _logger = logger;
        }

        public override ResultDto<RenovacaoEncaminhamentoDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<RenovacaoEncaminhamentoDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select Id
                                        ,EncaminhamentoId
                                        ,DataEncaminhamento
                                        ,QuemAutorizou
                                        ,Validada
                                        ,Ativo
                                   FROM renovacaoencaminhamento
                                  order by DataEncaminhamento desc";

                DataTable result = _renovacaoEncaminhamentoRepository.GetAll(selectQuery);
                List<RenovacaoEncaminhamento> list = result.CreateListFromTable<RenovacaoEncaminhamento>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<RenovacaoEncaminhamento>, IEnumerable<RenovacaoEncaminhamentoDto>>(list ?? Enumerable.Empty<RenovacaoEncaminhamento>());
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

        public override ResultDto<RenovacaoEncaminhamentoDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<RenovacaoEncaminhamentoDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select Id
                                        ,EncaminhamentoId
                                        ,DataEncaminhamento
                                        ,QuemAutorizou
                                        ,Validada
                                        ,Ativo
                                   FROM renovacaoencaminhamento
                                  WHERE Id = {id};";

                DataTable result = _renovacaoEncaminhamentoRepository.Get(selectQuery);
                List<RenovacaoEncaminhamento> item = result.CreateListFromTable<RenovacaoEncaminhamento>();

                if (item?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = item.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<RenovacaoEncaminhamento>, IEnumerable<RenovacaoEncaminhamentoDto>>(item ?? Enumerable.Empty<RenovacaoEncaminhamento>());
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

        public override ResultDto<RenovacaoEncaminhamentoDto> Insert(RenovacaoEncaminhamentoDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<RenovacaoEncaminhamentoDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<RenovacaoEncaminhamentoDto, RenovacaoEncaminhamento>(dto);
                insertQuery = $@"INSERT INTO renovacaoencaminhamento 
                                 (EncaminhamentoId, DataEncaminhamento, QuemAutorizou, Validada, Ativo)
                                 VALUES 
                                 ({entidade.EncaminhamentoId}, '{entidade.DataEncaminhamento:yyyy-MM-dd}', '{entidade.QuemAutorizou}', {entidade.Validada}, true);";

                long newId = _renovacaoEncaminhamentoRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<RenovacaoEncaminhamento, RenovacaoEncaminhamentoDto>(entidade);

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

        public override ResultDto<RenovacaoEncaminhamentoDto> Update(RenovacaoEncaminhamentoDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<RenovacaoEncaminhamentoDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<RenovacaoEncaminhamentoDto, RenovacaoEncaminhamento>(dto);
                updateQuery = $@"UPDATE renovacaoencaminhamento 
                                 SET DataEncaminhamento = '{entidade.DataEncaminhamento:yyyy-MM-dd}' 
                                 ,QuemAutorizou = '{entidade.QuemAutorizou}' 
                                 ,Validada = {entidade.Validada}
                                 ,Ativo = {entidade.Ativo}
                                 WHERE id = {entidade.Id};";

                _renovacaoEncaminhamentoRepository.Update(updateQuery);
                var item = _mapper.Map<RenovacaoEncaminhamento, RenovacaoEncaminhamentoDto>(entidade);

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
