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
    public class UfService : BaseService<UfDto, Uf>, IUfService
    {
        private readonly IUfRepository _ufRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UfService> _logger;

        public UfService(IUfRepository ufRepository,
                         ILogger<UfService> logger,
                         IMapper mapper,
                         IConfiguration configuration) : base(ufRepository, mapper)
        {
            _ufRepository = ufRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<UfDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<UfDto> returnValue = new();

            try
            {
                string selectQuery = $@"SELECT Id, Sigla, Nome FROM uf ORDER BY Sigla;";

                DataTable result = _ufRepository.GetAll(selectQuery);
                List<Uf> list = result.CreateListFromTable<Uf>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Uf>, IEnumerable<UfDto>>(list ?? Enumerable.Empty<Uf>());
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
