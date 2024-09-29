using AutoMapper;
using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Library.Extensions;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.Repositories.Implementations;
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

        public override ResultDto<GrupoGuiaDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<GrupoGuiaDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, Nome
                                 FROM grupoguia
                                 WHERE id = {id};";

                DataTable result = _grupoGuiaRepository.Get(selectQuery);
                List<GrupoGuia> item = result.CreateListFromTable<GrupoGuia>();

                if (item?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = item.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<GrupoGuia>, IEnumerable<GrupoGuiaDto>>(item ?? Enumerable.Empty<GrupoGuia>());
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

        public override ResultDto<GrupoGuiaDto> Insert(GrupoGuiaDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<GrupoGuiaDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<GrupoGuiaDto, GrupoGuia>(dto);
                insertQuery = $@"INSERT INTO grupoguia 
                                 (Nome)
                                 VALUES 
                                 ('{entidade.Nome}');";

                long newId = _grupoGuiaRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                GrupoGuiaDto? item = _mapper.Map<GrupoGuia, GrupoGuiaDto>(entidade);

                returnValue.Items = returnValue.Items.Concat(new[] { item });
                returnValue.WasExecuted = true;
                returnValue.ResponseCode = 200;
            }
            catch (Exception ex)
            {
                returnValue.BindError(500, ex.GetErrorMessage());
                LogContext.PushProperty("Query", insertQuery);
                _logger.LogError(ex, "Erro na gravação dos dados");
            }

            elapsedTime.Stop();
            returnValue.ElapsedTime = elapsedTime.Elapsed;

            return returnValue;
        }

        public override ResultDto<GrupoGuiaDto> Update(GrupoGuiaDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<GrupoGuiaDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<GrupoGuiaDto, GrupoGuia>(dto);
                updateQuery = $@"UPDATE grupoguia 
                                 SET Nome = '{entidade.Nome}'
                                 WHERE id = {entidade.Id};";

                _grupoGuiaRepository.Update(updateQuery);
                var item = _mapper.Map<GrupoGuia, GrupoGuiaDto>(entidade);

                returnValue.Items = returnValue.Items.Concat(new[] { item });
                returnValue.WasExecuted = true;
                returnValue.ResponseCode = 200;
            }
            catch (Exception ex)
            {
                returnValue.BindError(500, ex.GetErrorMessage());
                LogContext.PushProperty("Query", updateQuery);
                _logger.LogError(ex, "Erro na gravação dos dados");
            }

            elapsedTime.Stop();
            returnValue.ElapsedTime = elapsedTime.Elapsed;

            return returnValue;
        }
    }
}
