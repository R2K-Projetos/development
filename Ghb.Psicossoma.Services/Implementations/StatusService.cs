using AutoMapper;
using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Library.Extensions;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Diagnostics;

namespace Ghb.Psicossoma.Services.Implementations
{
    public class StatusService : BaseService<StatusUsuarioDto, StatusUsuario>, IStatusService
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<StatusService> _logger;

        public StatusService(IStatusRepository statusRepository,
                             ILogger<StatusService> logger,
                             IMapper mapper,
                             IConfiguration configuration) : base(statusRepository, mapper)
        {
            _statusRepository = statusRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<StatusUsuarioDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<StatusUsuarioDto> returnValue = new();

            try
            {
                string selectQuery = $@"SELECT Id, Descricao FROM status;";

                DataTable result = _statusRepository.GetAll(selectQuery);
                List<StatusUsuario> list = result.CreateListFromTable<StatusUsuario>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<StatusUsuario>, IEnumerable<StatusUsuarioDto>>(list ?? Enumerable.Empty<StatusUsuario>());
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
                _logger.LogError(ex, "Erro na recuperação dos dados");
            }

            elapsedTime.Stop();
            returnValue.ElapsedTime = elapsedTime.Elapsed;

            return returnValue;
        }
    }
}
