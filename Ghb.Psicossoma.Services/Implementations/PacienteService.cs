using AutoMapper;
using System.Data;
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
    public class PacienteService : BaseService<PacienteDto, Paciente>, IPacienteService
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PacienteService> _logger;

        public PacienteService(IPacienteRepository pacienteRepository,
                               ILogger<PacienteService> logger,
                               IMapper mapper,
                               IConfiguration configuration) : base(pacienteRepository, mapper)
        {
            _pacienteRepository = pacienteRepository;
            _configuration = configuration;
            _logger = logger;
        }

        ResultDto<PacienteResponseDto> IPacienteService.Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PacienteResponseDto> returnValue = new();

            try
            {
                string selectQuery = $@"SELECT pac.Id, ps.Nome, ps.Email, pac.Ativo
                                        FROM paciente pac
                                        INNER JOIN pessoa ps ON pac.PessoaId = ps.Id
                                        WHERE pac.Id = {id};";

                DataTable result = _pacienteRepository.Get(selectQuery);
                List<PacienteResponse> users = result.CreateListFromTable<PacienteResponse>();

                if (users?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = users.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<PacienteResponse>, IEnumerable<PacienteResponseDto>>(users ?? Enumerable.Empty<PacienteResponse>());
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

        ResultDto<PacienteResponseDto> IPacienteService.GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PacienteResponseDto> returnValue = new();

            try
            {
                string selectQuery = $@"SELECT pac.Id, ps.Nome, ps.Email, pac.Ativo
                                        FROM paciente pac
                                        INNER JOIN pessoa ps ON pac.PessoaId = ps.Id;";

                DataTable result = _pacienteRepository.GetAll(selectQuery);
                List<PacienteResponse> users = result.CreateListFromTable<PacienteResponse>();

                if (users?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = users.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<PacienteResponse>, IEnumerable<PacienteResponseDto>>(users ?? Enumerable.Empty<PacienteResponse>());
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
