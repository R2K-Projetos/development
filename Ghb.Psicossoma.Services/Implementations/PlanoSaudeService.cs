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

        public override ResultDto<PlanoSaudeDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PlanoSaudeDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, Descricao 
                                   FROM planoSaude
                                  WHERE id = {id};";

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

        public override ResultDto<PlanoSaudeDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PlanoSaudeDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, Descricao FROM planoSaude;";

                DataTable result = _planoSaudeRepository.GetAll(selectQuery);
                List<PlanoSaude> itens = result.CreateListFromTable<PlanoSaude>();

                if (itens?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = itens.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<PlanoSaude>, IEnumerable<PlanoSaudeDto>>(itens ?? Enumerable.Empty<PlanoSaude>());
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
    }
}
