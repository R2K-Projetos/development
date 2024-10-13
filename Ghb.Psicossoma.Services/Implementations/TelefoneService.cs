using AutoMapper;
using System.Data;
using Serilog.Context;
using System.Diagnostics;
using Ghb.Psicossoma.Services.Dtos;
using Microsoft.Extensions.Logging;
using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Library.Extensions;
using Microsoft.Extensions.Configuration;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

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

        public override ResultDto<TelefoneDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<TelefoneDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, PessoaId, TipoTelefoneId, Principal, Dddnum, Ativo
                                 FROM telefone
                                 WHERE Id = {id};";

                DataTable result = _telefoneRepository.Get(selectQuery);
                List<Telefone> telefones = result.CreateListFromTable<Telefone>();

                if (telefones?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = telefones.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Telefone>, IEnumerable<TelefoneDto>>(telefones ?? Enumerable.Empty<Telefone>());
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

        public override ResultDto<TelefoneDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<TelefoneDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, PessoaId, TipoTelefoneId, Principal, Dddnum, Ativo
                                 FROM telefone;";

                DataTable result = _telefoneRepository.GetAll(selectQuery);
                List<Telefone> telefones = result.CreateListFromTable<Telefone>();

                if (telefones?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = telefones.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Telefone>, IEnumerable<TelefoneDto>>(telefones ?? Enumerable.Empty<Telefone>());
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
