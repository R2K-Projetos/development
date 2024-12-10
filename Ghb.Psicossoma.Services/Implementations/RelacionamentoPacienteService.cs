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
    public class RelacionamentoPacienteService : BaseService<RelacionamentoPacienteDto, RelacionamentoPaciente>, IRelacionamentoPacienteService
    {
        private readonly IRelacionamentoPacienteRepository _relacionamentoPacienteRepository;
        private readonly ILogger<RelacionamentoPacienteService> _logger;

        public RelacionamentoPacienteService(IRelacionamentoPacienteRepository relacionamentoPacienteRepository,
                               ILogger<RelacionamentoPacienteService> logger,
                               IMapper mapper) : base(relacionamentoPacienteRepository, mapper)
        {
            _relacionamentoPacienteRepository = relacionamentoPacienteRepository;
            _logger = logger;
        }

        public ResultDto<RelacionamentoPacienteDto> GetRelacionamentoPaciente(string Id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<RelacionamentoPacienteDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select rp.Id
                                        ,p.Nome as Dependente
                                        ,gp.Nome as GrauParentesco
                                        ,rp.ResponsavelId
                                        ,rp.DependenteId
                                   FROM relacionamentopaciente rp
                                  INNER JOIN pessoa p on p.Id = rp.DependenteId
                                  INNER JOIN grauparentesco gp on gp.Id = rp.GrauParentescoId
                                  WHERE rp.ResponsavelId = {Id}
                                  order by p.Nome";

                DataTable result = _relacionamentoPacienteRepository.GetAll(selectQuery);
                List<RelacionamentoPaciente> list = result.CreateListFromTable<RelacionamentoPaciente>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<RelacionamentoPaciente>, IEnumerable<RelacionamentoPacienteDto>>(list ?? Enumerable.Empty<RelacionamentoPaciente>());
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

        public ResultDto<RelacionamentoPacienteDto> AdicionaRelacionamentoPaciente(RelacionamentoPacienteDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<RelacionamentoPacienteDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<RelacionamentoPacienteDto, RelacionamentoPaciente>(dto);
                insertQuery = $@"INSERT INTO relacionamentopaciente
                                 (ResponsavelId, DependenteId, GrauParentescoId)
                                 VALUES
                                 ({dto.ResponsavelId},{dto.DependenteId},,{dto.GrauParentescoId}); ";

                long newId = _relacionamentoPacienteRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<RelacionamentoPaciente, RelacionamentoPacienteDto>(entidade);

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

        public ResultDto<RelacionamentoPacienteDto> RetiraRelacionamentoPaciente(RelacionamentoPacienteDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<RelacionamentoPacienteDto> returnValue = new();
            string? deleteQuery = null;

            try
            {
                deleteQuery = $@"DELETE FROM relacionamentopaciente
                                  WHERE ResponsavelId = {dto.ResponsavelId}
                                    AND DependenteId = {dto.DependenteId}; ";

                _relacionamentoPacienteRepository.Remove(deleteQuery);

                var item = new RelacionamentoPacienteDto();
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
