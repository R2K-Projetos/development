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
    public class GrupoGuiaService : BaseService<GrupoGuiaDto, GrupoGuia>, IGrupoGuiaService
    {
        private readonly IGrupoGuiaRepository _grupoGuiaRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GrupoGuiaService> _logger;

        public GrupoGuiaService(IGrupoGuiaRepository grupoGuiaRepository,
                             ILogger<GrupoGuiaService> logger,
                             IMapper mapper,
                             IConfiguration configuration) : base(grupoGuiaRepository, mapper)
        {
            _grupoGuiaRepository = grupoGuiaRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<GrupoGuiaDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<GrupoGuiaDto> returnValue = new();

            try
            {
                string selectQuery = $@"SELECT Id, Nome FROM grupoguia;";

                DataTable result = _grupoGuiaRepository.GetAll(selectQuery);
                List<GrupoGuia> list = result.CreateListFromTable<GrupoGuia>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<GrupoGuia>, IEnumerable<GrupoGuiaDto>>(list ?? Enumerable.Empty<GrupoGuia>());
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
