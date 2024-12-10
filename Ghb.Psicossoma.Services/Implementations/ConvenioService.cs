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
    public class ConvenioService : BaseService<ConvenioDto, Convenio>, IConvenioService
    {
        private readonly IConvenioRepository _convenioRepository;
        private readonly ILogger<ConvenioService> _logger;

        public ConvenioService(IConvenioRepository convenioRepository,
                               ILogger<ConvenioService> logger,
                               IMapper mapper) : base(convenioRepository, mapper)
        {
            _convenioRepository = convenioRepository;
            _logger = logger;
        }

        public override ResultDto<ConvenioDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ConvenioDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select Id
                                        ,Nome
                                        ,CNPJ
                                        ,Ativo
                                        ,DataCadastro
                                   FROM convenio
                                  order by Nome";

                DataTable result = _convenioRepository.GetAll(selectQuery);
                List<Convenio> list = result.CreateListFromTable<Convenio>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Convenio>, IEnumerable<ConvenioDto>>(list ?? Enumerable.Empty<Convenio>());
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

        public override ResultDto<ConvenioDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ConvenioDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select Id
                                        ,Nome
                                        ,CNPJ
                                        ,Ativo
                                        ,DataCadastro
                                   FROM convenio
                                  WHERE Id = {id};";

                DataTable result = _convenioRepository.Get(selectQuery);
                List<Convenio> item = result.CreateListFromTable<Convenio>();

                if (item?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = item.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Convenio>, IEnumerable<ConvenioDto>>(item ?? Enumerable.Empty<Convenio>());
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

        public override ResultDto<ConvenioDto> Insert(ConvenioDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ConvenioDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<ConvenioDto, Convenio>(dto);
                insertQuery = $@"INSERT INTO convenio 
                                 (Nome, CNPJ, Ativo, DataCadastro)
                                 VALUES 
                                 ('{entidade.Nome}', '{entidade.CNPJ}', true, Now());";

                long newId = _convenioRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<Convenio, ConvenioDto>(entidade);

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

        public override ResultDto<ConvenioDto> Update(ConvenioDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ConvenioDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<ConvenioDto, Convenio>(dto);
                updateQuery = $@"UPDATE convenio 
                                 SET Nome = '{entidade.Nome}', CNPJ = '{entidade.CNPJ}', Ativo = {entidade.Ativo}
                                 WHERE id = {entidade.Id};";

                _convenioRepository.Update(updateQuery);
                var item = _mapper.Map<Convenio, ConvenioDto>(entidade);

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
