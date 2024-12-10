using AutoMapper;
using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Library.Extensions;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Data;
using System.Diagnostics;

namespace Ghb.Psicossoma.Services.Implementations
{
    public class TipoArquivoService : BaseService<TipoArquivoDto, TipoArquivo>, ITipoArquivoService
    {
        private readonly ITipoArquivoRepository _tipoArquivoRepository;
        private readonly ILogger<TipoArquivoService> _logger;

        public TipoArquivoService(ITipoArquivoRepository tipoArquivoRepository,
                               ILogger<TipoArquivoService> logger,
                               IMapper mapper) : base(tipoArquivoRepository, mapper)
        {
            _tipoArquivoRepository = tipoArquivoRepository;
            _logger = logger;
        }

        public override ResultDto<TipoArquivoDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<TipoArquivoDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select Id
                                        ,Nome
                                   FROM tipoarquivo
                                  order by Nome";

                DataTable result = _tipoArquivoRepository.GetAll(selectQuery);
                List<TipoArquivo> list = result.CreateListFromTable<TipoArquivo>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<TipoArquivo>, IEnumerable<TipoArquivoDto>>(list ?? Enumerable.Empty<TipoArquivo>());
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
