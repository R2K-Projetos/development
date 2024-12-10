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
    public class LaudoAnamneseService : BaseService<LaudoAnamneseDto, LaudoAnamnese>, ILaudoAnamneseService
    {
        private readonly ILaudoAnamneseRepository _laudoAnamneseRepository;
        private readonly ILogger<LaudoAnamneseService> _logger;

        public LaudoAnamneseService(ILaudoAnamneseRepository laudoAnamneseRepository,
                       ILogger<LaudoAnamneseService> logger,
                       IMapper mapper) : base(laudoAnamneseRepository, mapper)
        {
            _laudoAnamneseRepository = laudoAnamneseRepository;
            _logger = logger;
        }

        public override ResultDto<LaudoAnamneseDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<LaudoAnamneseDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select Id
                                        ,EspecialidadeId
                                        ,PacienteId
                                        ,Descricao
                                        ,Tipo
                                        ,Ativo
                                   FROM laudoanamnese
                                  order by Tipo";

                DataTable result = _laudoAnamneseRepository.GetAll(selectQuery);
                List<LaudoAnamnese> laudoAnamneses = result.CreateListFromTable<LaudoAnamnese>();

                if (laudoAnamneses?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = laudoAnamneses.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<LaudoAnamnese>, IEnumerable<LaudoAnamneseDto>>(laudoAnamneses ?? Enumerable.Empty<LaudoAnamnese>());
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

        public override ResultDto<LaudoAnamneseDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<LaudoAnamneseDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select Id
                                        ,EspecialidadeId
                                        ,PacienteId
                                        ,Descricao
                                        ,Tipo
                                        ,Ativo
                                   FROM laudoanamnese
                                  WHERE Id = {id};";

                DataTable result = _laudoAnamneseRepository.Get(selectQuery);
                List<LaudoAnamnese> laudoAnamnese = result.CreateListFromTable<LaudoAnamnese>();

                if (laudoAnamnese?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = laudoAnamnese.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<LaudoAnamnese>, IEnumerable<LaudoAnamneseDto>>(laudoAnamnese ?? Enumerable.Empty<LaudoAnamnese>());
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

        public override ResultDto<LaudoAnamneseDto> Insert(LaudoAnamneseDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<LaudoAnamneseDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<LaudoAnamneseDto, LaudoAnamnese>(dto);
                insertQuery = $@"INSERT INTO laudoanamnese 
                                 (EspecialidadeId, PacienteId, Descricao, Tipo, Ativo)
                                 VALUES 
                                 ({entidade.EspecialidadeId}, {entidade.PacienteId}, '{entidade.Descricao}', '{entidade.Tipo}', {entidade.Ativo});";

                long newId = _laudoAnamneseRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<LaudoAnamnese, LaudoAnamneseDto>(entidade);

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

        public override ResultDto<LaudoAnamneseDto> Update(LaudoAnamneseDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<LaudoAnamneseDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<LaudoAnamneseDto, LaudoAnamnese>(dto);
                updateQuery = $@"UPDATE laudoanamnese 
                                 SET EspecialidadeId = {entidade.EspecialidadeId}
                                 ,Descricao = '{entidade.Descricao}' 
                                 ,Tipo = '{entidade.Tipo}' 
                                 ,Ativo = {entidade.Ativo}
                                 WHERE id = {entidade.Id};";

                _laudoAnamneseRepository.Update(updateQuery);
                var item = _mapper.Map<LaudoAnamnese, LaudoAnamneseDto>(entidade);

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
