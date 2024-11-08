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
                selectQuery = $@"SELECT e.Id
                                        ,e.PessoaId
                                        ,e.CidadeId
                                        ,e.CEP
                                        ,e.Logradouro
                                        ,e.Numero
                                        ,e.Complemento
                                        ,e.Bairro
                                        ,e.Ativo
                                   FROM endereco e
                                  WHERE e.PessoaId = {id};";

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
                selectQuery = $@"SELECT e.Id
                                        ,e.PessoaId
                                        ,e.CidadeId
                                        ,e.CEP
                                        ,e.Logradouro
                                        ,e.Numero
                                        ,e.Complemento
                                        ,e.Bairro
                                        ,e.Ativo
                                   FROM endereco e;";

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

        public override ResultDto<EnderecoDto> Insert(EnderecoDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<EnderecoDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                Endereco? endereco = _mapper.Map<EnderecoDto, Endereco>(dto);
                insertQuery = $@"INSERT INTO endereco(Id, PessoaId, CidadeId, Cep, Logradouro, Numero, Complemento, Bairro, Ativo)
                                 VALUES(null, {endereco.PessoaId}, {endereco.CidadeId}, '{endereco.Cep}', '{endereco.Logradouro}', '{endereco.Numero}', '{endereco.Complemento}', '{endereco.Bairro}', {endereco.Ativo});";

                long newId = _enderecoRepository.Insert(insertQuery);
                if (newId > 0)
                    endereco.Id = (int)newId;

                var item = _mapper.Map<Endereco, EnderecoDto>(endereco);

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

        public ResultDto<EnderecoDto> GetEnderecoPessoa(string PessoaId)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<EnderecoDto> returnValue = new();

            try
            {
                string selectQuery = $@"SELECT e.Id
                                               ,e.PessoaId
                                               ,e.CidadeId
                                               ,e.CEP
                                               ,e.Logradouro
                                               ,e.Numero
                                               ,e.Complemento
                                               ,e.Bairro
                                               ,e.Ativo
                                               ,c.UFId
                                          FROM endereco e
                                         INNER JOIN cidade c on c.Id = e.CidadeId
                                         WHERE e.PessoaId = {PessoaId};";

                DataTable result = _enderecoRepository.Get(selectQuery);
                List<EnderecoDto> list = result.CreateListFromTable<EnderecoDto>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = list.AsEnumerable<EnderecoDto>();
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
