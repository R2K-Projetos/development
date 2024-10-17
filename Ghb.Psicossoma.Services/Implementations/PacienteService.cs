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
                string selectQuery = $@"select pc.Id
                                               ,p.Id as PessoaId
                                               ,p.Nome
                                               ,p.NomeReduzido
                                               ,p.Cpf
                                               ,p.Sexo
                                               ,p.Email
                                               ,p.DataNascimento
                                               ,p.Ativo as IsAtivo
                                               ,pu.Descricao as PerfilUsuario
                                               ,st.Descricao as StatuslUsuario
                                          from paciente pc
                                         INNER JOIN pessoa p on p.Id = pc.pessoaId
                                          LEFT JOIN usuario u on u.PessoaId = p.Id
                                          LEFT JOIN perfilusuario pu on pu.Id = u.PerfilUsuarioId
                                          LEFT JOIN status st on st.Id = u.StatusId
                                         WHERE pc.Id = {id};";

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
                string selectQuery = $@"select pc.Id
                                               ,p.Id as PessoaId
                                               ,p.Nome
                                               ,p.NomeReduzido
                                               ,p.Cpf
                                               ,p.Sexo
                                               ,p.Email
                                               ,p.DataNascimento
                                               ,p.Ativo as IsAtivo
                                               ,pu.Descricao as PerfilUsuario
                                               ,st.Descricao as StatuslUsuario
                                          from paciente pc
                                         INNER JOIN pessoa p on p.Id = pc.pessoaId
                                          LEFT JOIN usuario u on u.PessoaId = p.Id
                                          LEFT JOIN perfilusuario pu on pu.Id = u.PerfilUsuarioId
                                          LEFT JOIN status st on st.Id = u.StatusId
                                         where 1 = 1
                                         order by p.Nome;";

                DataTable result = _pacienteRepository.GetAll(selectQuery);
                List<PacienteResponse> pacientes = result.CreateListFromTable<PacienteResponse>();

                if (pacientes?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = pacientes.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<PacienteResponse>, IEnumerable<PacienteResponseDto>>(pacientes ?? Enumerable.Empty<PacienteResponse>());
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
