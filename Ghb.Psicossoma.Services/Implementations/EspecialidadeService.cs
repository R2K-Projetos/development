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
    public class EspecialidadeService : BaseService<EspecialidadeDto, Especialidade>, IEspecialidadeService
    {
        private readonly IEspecialidadeRepository _especialidadeRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EspecialidadeService> _logger;

        public EspecialidadeService(IEspecialidadeRepository especialidadeRepository,
                                    ILogger<EspecialidadeService> logger,
                                    IMapper mapper,
                                    IConfiguration configuration) : base(especialidadeRepository, mapper)
        {
            _especialidadeRepository = especialidadeRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<EspecialidadeDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<EspecialidadeDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, Nome FROM especialidade;";

                DataTable result = _especialidadeRepository.GetAll(selectQuery);
                List<Especialidade> list = result.CreateListFromTable<Especialidade>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Especialidade>, IEnumerable<EspecialidadeDto>>(list ?? Enumerable.Empty<Especialidade>());
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

        public override ResultDto<EspecialidadeDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<EspecialidadeDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, Nome
                                 FROM especialidade
                                 WHERE id = {id};";

                DataTable result = _especialidadeRepository.Get(selectQuery);
                List<Especialidade> item = result.CreateListFromTable<Especialidade>();

                if (item?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = item.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Especialidade>, IEnumerable<EspecialidadeDto>>(item ?? Enumerable.Empty<Especialidade>());
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

        public override ResultDto<EspecialidadeDto> Insert(EspecialidadeDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<EspecialidadeDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<EspecialidadeDto, Especialidade>(dto);
                insertQuery = $@"INSERT INTO especialidade 
                                 (Nome)
                                 VALUES 
                                 ('{entidade.Nome}');";

                long newId = _especialidadeRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<Especialidade, EspecialidadeDto>(entidade);

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

        public override ResultDto<EspecialidadeDto> Update(EspecialidadeDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<EspecialidadeDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<EspecialidadeDto, Especialidade>(dto);
                updateQuery = $@"UPDATE especialidade 
                                 SET Nome = '{entidade.Nome}'
                                 WHERE id = 
                {entidade.Id};";

                _especialidadeRepository.Update(updateQuery);
                var item = _mapper.Map<Especialidade, EspecialidadeDto>(entidade);

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

        public ResultDto<EspecialidadeResponseDto> GetEspecialidadeDisponivel(string ProfissionalId)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<EspecialidadeResponseDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select e.Id
                                       ,e.Nome
                                       ,IF(IFNULL(pe.Id, 0) = 0, '0', '1') as Checked
                                  from especialidade e
                                  left join profissionalespecialidade pe on pe.EspecialidadeId = e.Id
                                   and pe.ProfissionalId = {ProfissionalId}
                                 order by 2";

                DataTable result = _especialidadeRepository.GetAll(selectQuery);
                List<EspecialidadeResponseDto> list = result.CreateListFromTable<EspecialidadeResponseDto>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = list.AsEnumerable();
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

        public ResultDto<ProfissionalEspecialidadeDto> AdicionaEspecialidade(ProfissionalEspecialidadeDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProfissionalEspecialidadeDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<ProfissionalEspecialidadeDto, ProfissionalEspecialidade>(dto);
                insertQuery = $@"INSERT INTO profissionalespecialidade
                                 (ProfissionalId, EspecialidadeId, Ativo)
                                 VALUES
                                 ('{dto.ProfissionalId}','{dto.EspecialidadeId}',1); ";

                long newId = _especialidadeRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<ProfissionalEspecialidade, ProfissionalEspecialidadeDto>(entidade);

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

        public ResultDto<ProfissionalEspecialidadeDto> RetiraEspecialidade(ProfissionalEspecialidadeDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProfissionalEspecialidadeDto> returnValue = new();
            string? deleteQuery = null;

            try
            {
                deleteQuery = $@"DELETE FROM profissionalespecialidade
                                  WHERE ProfissionalId = {dto.ProfissionalId}
                                    AND EspecialidadeId = {dto.EspecialidadeId}; ";

                _especialidadeRepository.Remove(deleteQuery);

                var item = new ProfissionalEspecialidadeDto();
                returnValue.Items = returnValue.Items.Concat(new[] { item });
                returnValue.WasExecuted = true;
                returnValue.ResponseCode = 200;
            }
            catch (Exception ex)
            {
                returnValue.BindError(500, ex.GetErrorMessage());
                LogContext.PushProperty("Query", deleteQuery);
                _logger.LogError(ex, "Erro na exclusão dos dados");
            }

            elapsedTime.Stop();
            returnValue.ElapsedTime = elapsedTime.Elapsed;

            return returnValue;
        }
    }
}
