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
    public class PerfilUsuarioService : BaseService<PerfilUsuarioDto, PerfilUsuario>, IPerfilUsuarioService
    {
        private readonly IPerfilUsuarioRepository _perfilUsuarioRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PerfilUsuarioService> _logger;

        public PerfilUsuarioService(IPerfilUsuarioRepository perfilUsuarioRepository,
                                    ILogger<PerfilUsuarioService> logger,
                                    IMapper mapper,
                                    IConfiguration configuration) : base(perfilUsuarioRepository, mapper)
        {
            _perfilUsuarioRepository = perfilUsuarioRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<PerfilUsuarioDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PerfilUsuarioDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, Nome FROM perfilUsuario;";

                DataTable result = _perfilUsuarioRepository.GetAll(selectQuery);
                List<PerfilUsuario> status = result.CreateListFromTable<PerfilUsuario>();

                if (status?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = status.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<PerfilUsuario>, IEnumerable<PerfilUsuarioDto>>(status ?? Enumerable.Empty<PerfilUsuario>());
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
