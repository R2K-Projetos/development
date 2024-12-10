using AutoMapper;
using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Library.Extensions;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Data;
using System.Diagnostics;
using System.Net;

namespace Ghb.Psicossoma.Services.Implementations
{
    public class PlanoSaudeService : BaseService<PlanoSaudeDto, PlanoSaude>, IPlanoSaudeService
    {
        private readonly IPlanoSaudeRepository _planoSaudeRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PlanoSaudeService> _logger;

        public PlanoSaudeService(IPlanoSaudeRepository planoConvenioRepository,
                                 ILogger<PlanoSaudeService> logger,
                                 IMapper mapper,
                                 IConfiguration configuration) : base(planoConvenioRepository, mapper)
        {
            _planoSaudeRepository = planoConvenioRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<PlanoSaudeDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PlanoSaudeDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id
                                        ,ConvenioId
                                        ,TipoAcomodacaoId
                                        ,Nome
                                        ,ProdutoPlano
                                        ,Acompanhante
                                        ,CodIdent
                                        ,CNS
                                        ,Cobertura
                                        ,Empresa
                                        ,Ativo
                                   FROM planoSaude
                                  order by Nome;";

                DataTable result = _planoSaudeRepository.GetAll(selectQuery);
                List<PlanoSaude> list = result.CreateListFromTable<PlanoSaude>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<PlanoSaude>, IEnumerable<PlanoSaudeDto>>(list ?? Enumerable.Empty<PlanoSaude>());
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

        public override ResultDto<PlanoSaudeDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PlanoSaudeDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id
                                        ,ConvenioId
                                        ,TipoAcomodacaoId
                                        ,Nome
                                        ,ProdutoPlano
                                        ,Acompanhante
                                        ,CodIdent
                                        ,CNS
                                        ,Cobertura
                                        ,Empresa
                                        ,Ativo
                                   FROM planoSaude
                                  WHERE Id = {id};";

                DataTable result = _planoSaudeRepository.Get(selectQuery);
                List<PlanoSaude> item = result.CreateListFromTable<PlanoSaude>();

                if (item?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = item.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<PlanoSaude>, IEnumerable<PlanoSaudeDto>>(item ?? Enumerable.Empty<PlanoSaude>());
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

        public override ResultDto<PlanoSaudeDto> Insert(PlanoSaudeDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PlanoSaudeDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<PlanoSaudeDto, PlanoSaude>(dto);
                insertQuery = $@"INSERT INTO planosaude 
                                 (ConvenioId
                                 ,TipoAcomodacaoId
                                 ,Nome
                                 ,ProdutoPlano
                                 ,Acompanhante
                                 ,CodIdent
                                 ,CNS
                                 ,Cobertura
                                 ,Empresa
                                 ,Ativo)
                                 VALUES 
                                 ({entidade.ConvenioId}
                                 ,{entidade.TipoAcomodacaoId}
                                 ,'{entidade.Nome}'
                                 ,'{entidade.ProdutoPlano}'
                                 ,{entidade.Acompanhante}
                                 ,'{entidade.CodIdent}'
                                 ,'{entidade.CNS}'
                                 ,'{entidade.Cobertura}'
                                 ,'{entidade.Empresa}'
                                 ,true)";

                long newId = _planoSaudeRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<PlanoSaude, PlanoSaudeDto>(entidade);

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

        public override ResultDto<PlanoSaudeDto> Update(PlanoSaudeDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PlanoSaudeDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<PlanoSaudeDto, PlanoSaude>(dto);
                updateQuery = $@"UPDATE planosaude 
                                 SET ConvenioId = {entidade.Nome}
                                 ,TipoAcomodacaoId = {entidade.TipoAcomodacaoId}
                                 ,Nome = '{entidade.Nome}'
                                 ,ProdutoPlano = '{entidade.ProdutoPlano}'
                                 ,Acompanhante = {entidade.Acompanhante}
                                 ,CodIdent = '{entidade.CodIdent}'
                                 ,CNS = '{entidade.CNS}'
                                 ,Cobertura = '{entidade.Cobertura}'
                                 ,Empresa = '{entidade.Empresa}'
                                 ,Ativo = {entidade.Ativo}
                                 WHERE id = {entidade.Id};";

                _planoSaudeRepository.Update(updateQuery);
                var item = _mapper.Map<PlanoSaude, PlanoSaudeDto>(entidade);

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
