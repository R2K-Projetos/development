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
    public class EnderecoService : BaseService<EnderecoDto, Endereco>, IEnderecoService
    {
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EnderecoService> _logger;

        public EnderecoService(IEnderecoRepository enderecoRepository,
                               ILogger<EnderecoService> logger,
                               IMapper mapper,
                               IConfiguration configuration) : base(enderecoRepository, mapper)
        {
            _enderecoRepository = enderecoRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<EnderecoDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<EnderecoDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, PessoaId, CidadeId, CEP, Logradouro, Numero, Complemento, Bairro, Ativo
                                 FROM endereco
                                 WHERE id = {id};";

                DataTable result = _enderecoRepository.Get(selectQuery);
                List<Endereco> enderecos = result.CreateListFromTable<Endereco>();

                if (enderecos?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = enderecos.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Endereco>, IEnumerable<EnderecoDto>>(enderecos ?? Enumerable.Empty<Endereco>());
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

        public override ResultDto<EnderecoDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<EnderecoDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, PessoaId, CidadeId, CEP, Logradouro, Numero, Complemento, Bairro, Ativo
                                 FROM endereco;";

                DataTable result = _enderecoRepository.GetAll(selectQuery);
                List<Endereco> enderecos = result.CreateListFromTable<Endereco>();

                if (enderecos?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = enderecos.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Endereco>, IEnumerable<EnderecoDto>>(enderecos ?? Enumerable.Empty<Endereco>());
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
