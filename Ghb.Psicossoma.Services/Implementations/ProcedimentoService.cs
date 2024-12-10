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
    public class ProcedimentoService : BaseService<ProcedimentoDto, Procedimento>, IProcedimentoService
    {
        private readonly IProcedimentoRepository _procedimentoRepository;
        private readonly ILogger<ProcedimentoService> _logger;

        public ProcedimentoService(IProcedimentoRepository procedimentoRepository,
                               ILogger<ProcedimentoService> logger,
                               IMapper mapper) : base(procedimentoRepository, mapper)
        {
            _procedimentoRepository = procedimentoRepository;
            _logger = logger;
        }

        public override ResultDto<ProcedimentoDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProcedimentoDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select Id
                                        ,EspecialidadeId
                                        ,Nome
                                        ,ValorBase
                                   FROM procedimento
                                  order by Nome";

                DataTable result = _procedimentoRepository.GetAll(selectQuery);
                List<Procedimento> list = result.CreateListFromTable<Procedimento>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Procedimento>, IEnumerable<ProcedimentoDto>>(list ?? Enumerable.Empty<Procedimento>());
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

        public override ResultDto<ProcedimentoDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProcedimentoDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select Id
                                        ,EspecialidadeId
                                        ,Nome
                                        ,ValorBase
                                   FROM procedimento
                                  WHERE Id = {id};";

                DataTable result = _procedimentoRepository.Get(selectQuery);
                List<Procedimento> item = result.CreateListFromTable<Procedimento>();

                if (item?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = item.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Procedimento>, IEnumerable<ProcedimentoDto>>(item ?? Enumerable.Empty<Procedimento>());
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

        public override ResultDto<ProcedimentoDto> Insert(ProcedimentoDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProcedimentoDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<ProcedimentoDto, Procedimento>(dto);
                insertQuery = $@"INSERT INTO procedimento 
                                 (EspecialidadeId, Nome, ValorBase)
                                 VALUES 
                                 ({entidade.EspecialidadeId}, '{entidade.Nome}', {entidade.ValorBase});";

                long newId = _procedimentoRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<Procedimento, ProcedimentoDto>(entidade);

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

        public override ResultDto<ProcedimentoDto> Update(ProcedimentoDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProcedimentoDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<ProcedimentoDto, Procedimento>(dto);
                updateQuery = $@"UPDATE procedimento 
                                 SET EspecialidadeId = {entidade.EspecialidadeId}
                                 ,Nome = '{entidade.Nome}' 
                                 ,ValorBase = {entidade.ValorBase}
                                 WHERE id = {entidade.Id};";

                _procedimentoRepository.Update(updateQuery);
                var item = _mapper.Map<Procedimento, ProcedimentoDto>(entidade);

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
