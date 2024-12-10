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
    public class TipoTelefoneService : BaseService<TipoTelefoneDto, TipoTelefone>, ITipoTelefoneService
    {
        private readonly ITipoTelefoneRepository _tipoTelefoneRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TipoTelefoneService> _logger;

        public TipoTelefoneService(ITipoTelefoneRepository tipoTelefoneRepository,
                             ILogger<TipoTelefoneService> logger,
                             IMapper mapper,
                             IConfiguration configuration) : base(tipoTelefoneRepository, mapper)
        {
            _tipoTelefoneRepository = tipoTelefoneRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<TipoTelefoneDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<TipoTelefoneDto> returnValue = new();

            try
            {
                string selectQuery = $@"SELECT Id, Nome 
                                        FROM tipoTelefone 
                                        order by Nome;";

                DataTable result = _tipoTelefoneRepository.GetAll(selectQuery);
                List<TipoTelefone> list = result.CreateListFromTable<TipoTelefone>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<TipoTelefone>, IEnumerable<TipoTelefoneDto>>(list ?? Enumerable.Empty<TipoTelefone>());
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

