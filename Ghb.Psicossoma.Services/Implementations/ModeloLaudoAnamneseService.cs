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
    public class ModeloLaudoAnamneseService : BaseService<ModeloLaudoAnamneseDto, ModeloLaudoAnamnese>, IModeloLaudoAnamneseService
    {
        private readonly IModeloLaudoAnamneseRepository _modeloLaudoAnamneseRepository;
        private readonly ILogger<ModeloLaudoAnamneseService> _logger;

        public ModeloLaudoAnamneseService(IModeloLaudoAnamneseRepository modeloLaudoAnamneseRepository,
                               ILogger<ModeloLaudoAnamneseService> logger,
                               IMapper mapper) : base(modeloLaudoAnamneseRepository, mapper)
        {
            _modeloLaudoAnamneseRepository = modeloLaudoAnamneseRepository;
            _logger = logger;
        }

        public override ResultDto<ModeloLaudoAnamneseDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ModeloLaudoAnamneseDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select Id
                                        ,EspecialidadeId
                                        ,Descricao
                                        ,Tipo
                                        ,Ativo
                                   FROM modelolaudoanamnese
                                  order by Tipo";

                DataTable result = _modeloLaudoAnamneseRepository.GetAll(selectQuery);
                List<ModeloLaudoAnamnese> list = result.CreateListFromTable<ModeloLaudoAnamnese>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<ModeloLaudoAnamnese>, IEnumerable<ModeloLaudoAnamneseDto>>(list ?? Enumerable.Empty<ModeloLaudoAnamnese>());
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

        public override ResultDto<ModeloLaudoAnamneseDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ModeloLaudoAnamneseDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select Id
                                        ,Nome
                                        ,CNPJ
                                        ,Ativo
                                        ,DataCadastro
                                   FROM modelolaudoanamnese
                                  WHERE Id = {id};";

                DataTable result = _modeloLaudoAnamneseRepository.Get(selectQuery);
                List<ModeloLaudoAnamnese> item = result.CreateListFromTable<ModeloLaudoAnamnese>();

                if (item?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = item.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<ModeloLaudoAnamnese>, IEnumerable<ModeloLaudoAnamneseDto>>(item ?? Enumerable.Empty<ModeloLaudoAnamnese>());
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

        public override ResultDto<ModeloLaudoAnamneseDto> Insert(ModeloLaudoAnamneseDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ModeloLaudoAnamneseDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<ModeloLaudoAnamneseDto, ModeloLaudoAnamnese>(dto);
                insertQuery = $@"INSERT INTO modeloLaudoAnamnese 
                                 (EspecialidadeId, Descricao, Tipo, Ativo)
                                 VALUES 
                                 ({entidade.EspecialidadeId}, '{entidade.Descricao}', '{entidade.Tipo}', true);";

                long newId = _modeloLaudoAnamneseRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<ModeloLaudoAnamnese, ModeloLaudoAnamneseDto>(entidade);

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

        public override ResultDto<ModeloLaudoAnamneseDto> Update(ModeloLaudoAnamneseDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ModeloLaudoAnamneseDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<ModeloLaudoAnamneseDto, ModeloLaudoAnamnese>(dto);
                updateQuery = $@"UPDATE modeloLaudoAnamnese 
                                 SET EspecialidadeId = {entidade.EspecialidadeId}
                                 ,Descricao = '{entidade.Descricao}'
                                 ,Tipo = '{entidade.Tipo}'
                                 ,Ativo = {entidade.Ativo}
                                 WHERE id = {entidade.Id};";

                _modeloLaudoAnamneseRepository.Update(updateQuery);
                var item = _mapper.Map<ModeloLaudoAnamnese, ModeloLaudoAnamneseDto>(entidade);

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
