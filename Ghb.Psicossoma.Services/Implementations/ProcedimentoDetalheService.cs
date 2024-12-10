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
    public class ProcedimentoDetalheService : BaseService<ProcedimentoDetalheDto, ProcedimentoDetalhe>, IProcedimentoDetalheService
    {
        private readonly IProcedimentoDetalheRepository _procedimentoDetalheRepository;
        private readonly ILogger<ProcedimentoDetalheService> _logger;

        public ProcedimentoDetalheService(IProcedimentoDetalheRepository procedimentoDetalheRepository,
                               ILogger<ProcedimentoDetalheService> logger,
                               IMapper mapper) : base(procedimentoDetalheRepository, mapper)
        {
            _procedimentoDetalheRepository = procedimentoDetalheRepository;
            _logger = logger;
        }

        public override ResultDto<ProcedimentoDetalheDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProcedimentoDetalheDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select Id
                                        ,ProcedimentoId
                                        ,Nome
                                        ,Aliquota
                                   FROM procedimentodetalhe
                                  order by Nome";

                DataTable result = _procedimentoDetalheRepository.GetAll(selectQuery);
                List<ProcedimentoDetalhe> list = result.CreateListFromTable<ProcedimentoDetalhe>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<ProcedimentoDetalhe>, IEnumerable<ProcedimentoDetalheDto>>(list ?? Enumerable.Empty<ProcedimentoDetalhe>());
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

        public override ResultDto<ProcedimentoDetalheDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProcedimentoDetalheDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select Id
                                        ,ProcedimentoId
                                        ,Nome
                                        ,Aliquota
                                   FROM procedimentodetalhe
                                  WHERE Id = {id};";

                DataTable result = _procedimentoDetalheRepository.Get(selectQuery);
                List<ProcedimentoDetalhe> item = result.CreateListFromTable<ProcedimentoDetalhe>();

                if (item?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = item.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<ProcedimentoDetalhe>, IEnumerable<ProcedimentoDetalheDto>>(item ?? Enumerable.Empty<ProcedimentoDetalhe>());
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

        public override ResultDto<ProcedimentoDetalheDto> Insert(ProcedimentoDetalheDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProcedimentoDetalheDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<ProcedimentoDetalheDto, ProcedimentoDetalhe>(dto);
                insertQuery = $@"INSERT INTO procedimentodetalhe 
                                 (ProcedimentoId, Nome, Aliquota)
                                 VALUES 
                                 ({entidade.ProcedimentoId}, '{entidade.Nome}', {entidade.Aliquota});";

                long newId = _procedimentoDetalheRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<ProcedimentoDetalhe, ProcedimentoDetalheDto>(entidade);

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

        public override ResultDto<ProcedimentoDetalheDto> Update(ProcedimentoDetalheDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProcedimentoDetalheDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<ProcedimentoDetalheDto, ProcedimentoDetalhe>(dto);
                updateQuery = $@"UPDATE procedimentodetalhe 
                                 SET ProcedimentoId = {entidade.ProcedimentoId} 
                                 ,Nome = '{entidade.Nome}' 
                                 ,Aliquota = {entidade.Aliquota}
                                 WHERE id = {entidade.Id};";

                _procedimentoDetalheRepository.Update(updateQuery);
                var item = _mapper.Map<ProcedimentoDetalhe, ProcedimentoDetalheDto>(entidade);

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
