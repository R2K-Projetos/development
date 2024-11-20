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
    public class TelefoneService : BaseService<TelefoneDto, Telefone>, ITelefoneService
    {
        private readonly ITelefoneRepository _telefoneRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TelefoneService> _logger;

        public TelefoneService(ITelefoneRepository telefoneRepository,
                               ILogger<TelefoneService> logger,
                               IMapper mapper,
                               IConfiguration configuration) : base(telefoneRepository, mapper)
        {
            _telefoneRepository = telefoneRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public ResultDto<TelefoneResponseDto> GetTelefone(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<TelefoneResponseDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT t.Id
                                        ,t.TipoTelefoneId
                                        ,t.Principal
                                        ,t.DDDNum
                                        ,t.Ativo
                                        ,tt.Nome as TipoTelefone
                                   FROM telefone t
                                  INNER JOIN TipoTelefone tt on tt.Id = t.TipoTelefoneId
                                  WHERE t.Id = {id};";

                DataTable result = _telefoneRepository.Get(selectQuery);
                List<TelefoneResponse> telefones = result.CreateListFromTable<TelefoneResponse>();

                if (telefones?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = telefones.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<TelefoneResponse>, IEnumerable<TelefoneResponseDto>>(telefones ?? Enumerable.Empty<TelefoneResponse>());
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

        public ResultDto<TelefoneResponseDto> GetAllTelefonePessoa(string PessaoId)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<TelefoneResponseDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT t.Id
                                        ,t.PessoaId                                        
                                        ,t.TipoTelefoneId
                                        ,t.Principal
                                        ,t.DDDNum
                                        ,t.Ativo
                                        ,tt.Nome as TipoTelefone
                                   FROM telefone t
                                  INNER JOIN TipoTelefone tt on tt.Id = t.TipoTelefoneId
                                  WHERE t.PessoaId = {PessaoId}
                                  ORDER BY IF(t.Principal = 1, 1, 2), t.Dddnum;";

                DataTable result = _telefoneRepository.GetAll(selectQuery);
                List<TelefoneResponse> telefones = result.CreateListFromTable<TelefoneResponse>();

                if (telefones?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = telefones.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<TelefoneResponse>, IEnumerable<TelefoneResponseDto>>(telefones ?? Enumerable.Empty<TelefoneResponse>());
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

        public override ResultDto<TelefoneDto> Insert(TelefoneDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<TelefoneDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                Telefone? telefone = _mapper.Map<TelefoneDto, Telefone>(dto);

                insertQuery = "";
                if (dto.Principal)
                {
                    insertQuery += $@"UPDATE telefone
                                      SET Principal = 0
                                      WHERE PessoaId = {telefone.PessoaId};";
                }
                insertQuery += $@"INSERT INTO telefone 
                                 (PessoaId, TipoTelefoneId, Principal, DDDNumero, Ativo)
                                 VALUES 
                                 ({telefone.PessoaId}, {telefone.TipoTelefoneId}, {telefone.Principal}, '{telefone.DDDNum}', {telefone.Ativo});";

                long newId = _telefoneRepository.Insert(insertQuery);
                if (newId > 0)
                    telefone.Id = (int)newId;

                var item = _mapper.Map<Telefone, TelefoneDto>(telefone);

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

        public override ResultDto<TelefoneDto> Update(TelefoneDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<TelefoneDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var telefone = _mapper.Map<TelefoneDto, Telefone>(dto);

                updateQuery = "";
                if (dto.Principal)
                {
                    updateQuery += $@"UPDATE telefone
                                         SET Principal = 0
                                       WHERE PessoaId = {telefone.PessoaId};";
                }
                updateQuery += $@"UPDATE telefone
                                    SET PessoaId = {telefone.PessoaId}
                                        ,TipoTelefoneId = {telefone.TipoTelefoneId}
                                        ,Principal = {telefone.Principal}
                                        ,DDDNum = '{telefone.DDDNum}'
                                        ,Ativo = {telefone.Ativo}
                                  WHERE Id = {telefone.Id};";

                _telefoneRepository.Update(updateQuery);
                var item = _mapper.Map<Telefone, TelefoneDto>(telefone);

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

        public ResultDto<TelefoneDto> Delete(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<TelefoneDto> returnValue = new();
            string? deleteQuery = null;

            try
            {
                deleteQuery += $@"DELETE FROM telefone
                                  WHERE Id = {id};";

                _telefoneRepository.Remove(deleteQuery);

                var item = new TelefoneDto();
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
