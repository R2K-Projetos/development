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
    public class GuiaAutorizacaoService : BaseService<GuiaAutorizacaoDto, GuiaAutorizacao>, IGuiaAutorizacaoService
    {
        private readonly IGuiaAutorizacaoRepository _guiaAutorizacaoRepository;
        private readonly ILogger<GuiaAutorizacaoService> _logger;

        public GuiaAutorizacaoService(IGuiaAutorizacaoRepository guiaAutorizacaoRepository,
                                      ILogger<GuiaAutorizacaoService> logger,
                                      IMapper mapper) : base(guiaAutorizacaoRepository, mapper)
        {
            _guiaAutorizacaoRepository = guiaAutorizacaoRepository;
            _logger = logger;
        }

        public override ResultDto<GuiaAutorizacaoDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<GuiaAutorizacaoDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select Id
                                        ,EncaminhamentoId
                                        ,GrupoGuiaId
                                        ,Numero
                                        ,DataEmissao
                                        ,ValorUnitario
                                        ,ValorTotal
                                        ,SessoesAutorizadas
                                        ,Ativo
                                   FROM guiaAutorizacao
                                  order by Nome";

                DataTable result = _guiaAutorizacaoRepository.GetAll(selectQuery);
                List<GuiaAutorizacao> convenios = result.CreateListFromTable<GuiaAutorizacao>();

                if (convenios?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = convenios.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<GuiaAutorizacao>, IEnumerable<GuiaAutorizacaoDto>>(convenios ?? Enumerable.Empty<GuiaAutorizacao>());
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

        public override ResultDto<GuiaAutorizacaoDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<GuiaAutorizacaoDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select Id
                                        ,EncaminhamentoId
                                        ,GrupoGuiaId
                                        ,Numero
                                        ,DataEmissao
                                        ,ValorUnitario
                                        ,ValorTotal
                                        ,SessoesAutorizadas
                                        ,Ativo
                                   FROM guiaAutorizacao
                                  WHERE Id = {id};";

                DataTable result = _guiaAutorizacaoRepository.Get(selectQuery);
                List<GuiaAutorizacao> guiaAutorizacao = result.CreateListFromTable<GuiaAutorizacao>();

                if (guiaAutorizacao?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = guiaAutorizacao.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<GuiaAutorizacao>, IEnumerable<GuiaAutorizacaoDto>>(guiaAutorizacao ?? Enumerable.Empty<GuiaAutorizacao>());
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

        public override ResultDto<GuiaAutorizacaoDto> Insert(GuiaAutorizacaoDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<GuiaAutorizacaoDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<GuiaAutorizacaoDto, GuiaAutorizacao>(dto);
                insertQuery = $@"INSERT INTO guiaAutorizacao 
                                 (EncaminhamentoId
                                 ,GrupoGuiaId
                                 ,Numero
                                 ,DataEmissao
                                 ,ValorUnitario
                                 ,ValorTotal
                                 ,SessoesAutorizadas
                                 ,Ativo)
                                 VALUES 
                                 ({entidade.EncaminhamentoId}
                                 ,{entidade.GrupoGuiaId}
                                 ,'{entidade.Numero}'
                                 ,'{entidade.DataEmissao:yyyy-MM-dd}'
                                 ,{entidade.ValorUnitario}
                                 ,{entidade.ValorTotal}
                                 ,{entidade.SessoesAutorizadas}
                                 ,true);";

                long newId = _guiaAutorizacaoRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<GuiaAutorizacao, GuiaAutorizacaoDto>(entidade);

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

        public override ResultDto<GuiaAutorizacaoDto> Update(GuiaAutorizacaoDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<GuiaAutorizacaoDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<GuiaAutorizacaoDto, GuiaAutorizacao>(dto);
                updateQuery = $@"UPDATE guiaAutorizacao 
                                 SET EncaminhamentoId = {entidade.EncaminhamentoId}
                                 ,GrupoGuiaId = {entidade.EncaminhamentoId}
                                 ,Numero = '{entidade.Numero}'
                                 ,DataEmissao = '{entidade.DataEmissao:yyyy-MM-dd}'
                                 ,ValorUnitario = {entidade.ValorUnitario}
                                 ,ValorTotal = {entidade.ValorTotal}
                                 ,SessoesAutorizadas = {entidade.SessoesAutorizadas}
                                 ,Ativo = {entidade.Ativo}
                                 WHERE id = {entidade.Id};";

                _guiaAutorizacaoRepository.Update(updateQuery);
                var item = _mapper.Map<GuiaAutorizacao, GuiaAutorizacaoDto>(entidade);

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
