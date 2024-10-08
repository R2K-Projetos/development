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
    public class CidadeService : BaseService<CidadeDto, Cidade>, ICidadeService
    {
        private readonly ICidadeRepository _cidadeRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CidadeService> _logger;

        public CidadeService(ICidadeRepository cidadeRepository,
                             ILogger<CidadeService> logger,
                             IMapper mapper,
                             IConfiguration configuration) : base(cidadeRepository, mapper)
        {
            _cidadeRepository = cidadeRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<CidadeDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<CidadeDto> returnValue = new();

            try
            {
                string selectQuery = $@"SELECT Id, UFId, Nome FROM cidade;";

                DataTable result = _cidadeRepository.GetAll(selectQuery);
                List<Cidade> list = result.CreateListFromTable<Cidade>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Cidade>, IEnumerable<CidadeDto>>(list ?? Enumerable.Empty<Cidade>());
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

        public override ResultDto<CidadeDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<CidadeDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, UFId, Nome FROM cidade
                                 WHERE id = {id};";

                DataTable result = _cidadeRepository.Get(selectQuery);
                List<Cidade> item = result.CreateListFromTable<Cidade>();

                if (item?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = item.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Cidade>, IEnumerable<CidadeDto>>(item ?? Enumerable.Empty<Cidade>());
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

        public ResultDto<CidadeDto> GetAllByUf(int ufId)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<CidadeDto> returnValue = new();

            try
            {
                string selectQuery = $@"SELECT Id, UFId, Nome FROM cidade
                                        WHERE ufid = {ufId};";

                DataTable result = _cidadeRepository.Get(selectQuery);
                List<Cidade> list = result.CreateListFromTable<Cidade>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Cidade>, IEnumerable<CidadeDto>>(list ?? Enumerable.Empty<Cidade>());
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

        public ResultDto<CidadeDto> GetAllByName(string name)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<CidadeDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, UFId, Nome
                                 FROM cidade
                                 WHERE Nome LIKE '%{name}%'
                                 LIMIT 5;";

                DataTable result = _cidadeRepository.Get(selectQuery);
                List<Cidade> pessoas = result.CreateListFromTable<Cidade>();

                if (pessoas?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = pessoas.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Cidade>, IEnumerable<CidadeDto>>(pessoas ?? Enumerable.Empty<Cidade>());
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
