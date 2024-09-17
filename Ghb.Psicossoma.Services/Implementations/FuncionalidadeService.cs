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
    public class FuncionalidadeService : BaseService<FuncionalidadeDto, Funcionalidade>, IFuncionalidadeService
    {
        private readonly IFuncionalidadeRepository _funcionalidadeRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<FuncionalidadeService> _logger;


        public FuncionalidadeService(IFuncionalidadeRepository funcionalidadeRepository,
                             ILogger<FuncionalidadeService> logger,
                             IMapper mapper,
                             IConfiguration configuration) : base(funcionalidadeRepository, mapper)
        {
            _funcionalidadeRepository = funcionalidadeRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<FuncionalidadeDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<FuncionalidadeDto> returnValue = new();

            try
            {
                string selectQuery = $@"SELECT Id, Nome FROM funcionalidades;";

                DataTable result = _funcionalidadeRepository.GetAll(selectQuery);
                List<Funcionalidade> list = result.CreateListFromTable<Funcionalidade>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Funcionalidade>, IEnumerable<FuncionalidadeDto>>(list ?? Enumerable.Empty<Funcionalidade>());
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
