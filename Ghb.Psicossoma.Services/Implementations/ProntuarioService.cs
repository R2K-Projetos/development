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
    public class ProntuarioService : BaseService<ProntuarioDto, Prontuario>, IProntuarioService
    {
        private readonly IProntuarioRepository _prontuarioRepository;
        private readonly ILogger<ProntuarioService> _logger;

        public ProntuarioService(IProntuarioRepository prontuarioRepository,
                                 ILogger<ProntuarioService> logger,
                                 IMapper mapper) : base(prontuarioRepository, mapper)
        {
            _prontuarioRepository = prontuarioRepository;
            _logger = logger;
        }

        public override ResultDto<ProntuarioDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProntuarioDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id
                                        ,EncaminhamentoId
                                        ,ProfissionalId
                                        ,LaudoAnamneseId
                                        ,DataEntrada
                                        ,Ativo
                                   FROM prontuario;";

                DataTable result = _prontuarioRepository.GetAll(selectQuery);
                List<Prontuario> list = result.CreateListFromTable<Prontuario>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Prontuario>, IEnumerable<ProntuarioDto>>(list ?? Enumerable.Empty<Prontuario>());
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

        public override ResultDto<ProntuarioDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProntuarioDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id
                                        ,EncaminhamentoId
                                        ,ProfissionalId
                                        ,LaudoAnamneseId
                                        ,DataEntrada
                                        ,Ativo
                                   FROM prontuario
                                  WHERE Id = {id};";

                DataTable result = _prontuarioRepository.Get(selectQuery);
                List<Prontuario> item = result.CreateListFromTable<Prontuario>();

                if (item?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = item.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Prontuario>, IEnumerable<ProntuarioDto>>(item ?? Enumerable.Empty<Prontuario>());
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

        public override ResultDto<ProntuarioDto> Insert(ProntuarioDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProntuarioDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<ProntuarioDto, Prontuario>(dto);
                insertQuery = $@"INSERT INTO prontuario 
                                 (EncaminhamentoId, ProfissionalId, LaudoAnamneseId, DataEntrada, Ativo)
                                 VALUES 
                                 ({entidade.EncaminhamentoId}, {entidade.ProfissionalId}, {entidade.LaudoAnamneseId}, '{entidade.DataEntrada:yyyy-MM-dd}', true);";

                long newId = _prontuarioRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<Prontuario, ProntuarioDto>(entidade);

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

        public override ResultDto<ProntuarioDto> Update(ProntuarioDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProntuarioDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<ProntuarioDto, Prontuario>(dto);
                updateQuery = $@"UPDATE prontuario 
                                 SET EncaminhamentoId = '{entidade.EncaminhamentoId}'
                                 ,ProfissionalId = '{entidade.ProfissionalId}'
                                 ,LaudoAnamneseId = '{entidade.LaudoAnamneseId}'
                                 ,DataEntrada = '{entidade.DataEntrada:yyyy-MM-dd}'
                                 ,Ativo = {entidade.Ativo}
                                 WHERE id = {entidade.Id};";

                _prontuarioRepository.Update(updateQuery);
                var item = _mapper.Map<Prontuario, ProntuarioDto>(entidade);

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
