using AutoMapper;
using System.Data;
using Serilog.Context;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Library.Extensions;
using Microsoft.Extensions.Configuration;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Services.Implementations
{
    public class PlanoConvenioService : BaseService<PlanoConvenioDto, PlanoConvenio>, IPlanoConvenioService
    {
        private readonly IPlanoConvenioRepository _planoConvenioRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PlanoConvenioService> _logger;

        public PlanoConvenioService(IPlanoConvenioRepository planoConvenioRepository,
                                    ILogger<PlanoConvenioService> logger,
                                    IMapper mapper,
                                    IConfiguration configuration) : base(planoConvenioRepository, mapper)
        {
            _planoConvenioRepository = planoConvenioRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<PlanoConvenioDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PlanoConvenioDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, Descricao FROM planoConvenio;";

                DataTable result = _planoConvenioRepository.GetAll(selectQuery);
                List<PlanoConvenio> planos = result.CreateListFromTable<PlanoConvenio>();

                if (planos?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = planos.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<PlanoConvenio>, IEnumerable<PlanoConvenioDto>>(planos ?? Enumerable.Empty<PlanoConvenio>());
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
