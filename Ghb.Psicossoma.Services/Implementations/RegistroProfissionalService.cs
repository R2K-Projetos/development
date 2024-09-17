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
    public class RegistroProfissionalService : BaseService<RegistroProfissionalDto, RegistroProfissional>, IRegistroProfissionalService
    {
        private readonly IRegistroProfissionalRepository _registroProfissionalRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RegistroProfissionalService> _logger;

        public RegistroProfissionalService(IRegistroProfissionalRepository registroProfissionalRepository,
                                           ILogger<RegistroProfissionalService> logger,
                                           IMapper mapper,
                                           IConfiguration configuration) : base(registroProfissionalRepository, mapper)
        {
            _registroProfissionalRepository = registroProfissionalRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<RegistroProfissionalDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<RegistroProfissionalDto> returnValue = new();

            try
            {
                string selectQuery = $@"SELECT Id, Nome FROM registroProfissional;";

                DataTable result = _registroProfissionalRepository.GetAll(selectQuery);
                List<RegistroProfissional> status = result.CreateListFromTable<RegistroProfissional>();

                if (status?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = status.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<RegistroProfissional>, IEnumerable<RegistroProfissionalDto>>(status ?? Enumerable.Empty<RegistroProfissional>());
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
