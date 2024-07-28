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
    public class ProntuarioService : BaseService<ProntuarioDto, Prontuario>, IProntuarioService
    {
        private readonly IProntuarioRepository _prontuarioRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProntuarioService> _logger;

        public ProntuarioService(IProntuarioRepository prontuarioRepository,
                                 ILogger<ProntuarioService> logger,
                                 IMapper mapper,
                                 IConfiguration configuration) : base(prontuarioRepository, mapper)
        {
            _prontuarioRepository = prontuarioRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<ProntuarioDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProntuarioDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, EncaminhamentoId, ProfissionalId, PacienteId, DescricaoGeral, Data, Ativo
                                 FROM prontuario
                                 WHERE Id = {id};";

                DataTable result = _prontuarioRepository.Get(selectQuery);
                List<Prontuario> prontuarios = result.CreateListFromTable<Prontuario>();

                if (prontuarios?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = prontuarios.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Prontuario>, IEnumerable<ProntuarioDto>>(prontuarios ?? Enumerable.Empty<Prontuario>());
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

        public override ResultDto<ProntuarioDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProntuarioDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, EncaminhamentoId, ProfissionalId, PacienteId, DescricaoGeral, Data, Ativo
                                 FROM prontuario;";

                DataTable result = _prontuarioRepository.GetAll(selectQuery);
                List<Prontuario> prontuarios = result.CreateListFromTable<Prontuario>();

                if (prontuarios?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = prontuarios.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Prontuario>, IEnumerable<ProntuarioDto>>(prontuarios ?? Enumerable.Empty<Prontuario>());
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
