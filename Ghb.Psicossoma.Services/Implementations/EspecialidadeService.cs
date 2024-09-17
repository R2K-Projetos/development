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
    public class EspecialidadeService : BaseService<EspecialidadeDto, Especialidade>, IEspecialidadeService
    {
        private readonly IEspecialidadeRepository _especialidadeRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EspecialidadeService> _logger;

        public EspecialidadeService(IEspecialidadeRepository especialidadeRepository,
                                    ILogger<EspecialidadeService> logger,
                                    IMapper mapper,
                                    IConfiguration configuration) : base(especialidadeRepository, mapper)
        {
            _especialidadeRepository = especialidadeRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<EspecialidadeDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<EspecialidadeDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, Nome FROM especialidade;";

                DataTable result = _especialidadeRepository.GetAll(selectQuery);
                List<Especialidade> status = result.CreateListFromTable<Especialidade>();

                if (status?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = status.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Especialidade>, IEnumerable<EspecialidadeDto>>(status ?? Enumerable.Empty<Especialidade>());
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
