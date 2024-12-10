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
    public class TipoAcomodacaoService : BaseService<TipoAcomodacaoDto, TipoAcomodacao>, ITipoAcomodacaoService
    {
        private readonly ITipoAcomodacaoRepository _tipoAcomodacaoRepository;
        private readonly ILogger<TipoAcomodacaoService> _logger;

        public TipoAcomodacaoService(ITipoAcomodacaoRepository tipoAcomodacaoRepository,
                               ILogger<TipoAcomodacaoService> logger,
                               IMapper mapper) : base(tipoAcomodacaoRepository, mapper)
        {
            _tipoAcomodacaoRepository = tipoAcomodacaoRepository;
            _logger = logger;
        }

        public override ResultDto<TipoAcomodacaoDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<TipoAcomodacaoDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select Id
                                        ,Codigo
                                        ,Nome
                                   FROM tipoacomodacao
                                  order by Nome";

                DataTable result = _tipoAcomodacaoRepository.GetAll(selectQuery);
                List<TipoAcomodacao> list = result.CreateListFromTable<TipoAcomodacao>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<TipoAcomodacao>, IEnumerable<TipoAcomodacaoDto>>(list ?? Enumerable.Empty<TipoAcomodacao>());
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

        public override ResultDto<TipoAcomodacaoDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<TipoAcomodacaoDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select Id
                                        ,Codigo
                                        ,Nome
                                   FROM tipoacomodacao
                                  WHERE Id = {id};";

                DataTable result = _tipoAcomodacaoRepository.Get(selectQuery);
                List<TipoAcomodacao> item = result.CreateListFromTable<TipoAcomodacao>();

                if (item?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = item.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<TipoAcomodacao>, IEnumerable<TipoAcomodacaoDto>>(item ?? Enumerable.Empty<TipoAcomodacao>());
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

        public override ResultDto<TipoAcomodacaoDto> Insert(TipoAcomodacaoDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<TipoAcomodacaoDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<TipoAcomodacaoDto, TipoAcomodacao>(dto);
                insertQuery = $@"INSERT INTO tipoacomodacao 
                                 (Codigo, Nome)
                                 VALUES 
                                 ('{entidade.Codigo}', '{entidade.Nome}');";

                long newId = _tipoAcomodacaoRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<TipoAcomodacao, TipoAcomodacaoDto>(entidade);

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

        public override ResultDto<TipoAcomodacaoDto> Update(TipoAcomodacaoDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<TipoAcomodacaoDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<TipoAcomodacaoDto, TipoAcomodacao>(dto);
                updateQuery = $@"UPDATE tipoacomodacao 
                                 SET Codigo = '{entidade.Codigo}', Nome = '{entidade.Nome}'
                                 WHERE Id = {entidade.Id};";

                _tipoAcomodacaoRepository.Update(updateQuery);
                var item = _mapper.Map<TipoAcomodacao, TipoAcomodacaoDto>(entidade);

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
