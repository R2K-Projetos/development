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
    public class GrauParentescoService : BaseService<GrauParentescoDto, GrauParentesco>, IGrauParentescoService
    {
        private readonly IGrauParentescoRepository _grauParentescoRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GrauParentescoService> _logger;

        public GrauParentescoService(IGrauParentescoRepository grauParentescoRepository,
                             ILogger<GrauParentescoService> logger,
                             IMapper mapper,
                             IConfiguration configuration) : base(grauParentescoRepository, mapper)
        {
            _grauParentescoRepository = grauParentescoRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<GrauParentescoDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<GrauParentescoDto> returnValue = new();

            try
            {
                string selectQuery = $@"SELECT Id, Nome FROM grauparentesco;";

                DataTable result = _grauParentescoRepository.GetAll(selectQuery);
                List<GrauParentesco> list = result.CreateListFromTable<GrauParentesco>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<GrauParentesco>, IEnumerable<GrauParentescoDto>>(list ?? Enumerable.Empty<GrauParentesco>());
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
