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
    public class ProfissionalService : BaseService<ProfissionalDto, Profissional>, IProfissionalService
    {
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly IPessoaService _pessoaService;
        private readonly IEnderecoService _enderecoService;
        private readonly ITelefoneService _telefoneService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProfissionalService> _logger;

        public ProfissionalService(IProfissionalRepository profissionalRepository,
                                   IPessoaService pessoaService,
                                   IEnderecoService enderecoService,
                                   ITelefoneService telefoneService,
                                   ILogger<ProfissionalService> logger,
                                   IMapper mapper,
                                   IConfiguration configuration) : base(profissionalRepository, mapper)
        {
            _logger = logger;
            _profissionalRepository = profissionalRepository;
            _pessoaService = pessoaService;
            _enderecoService = enderecoService;
            _telefoneService = telefoneService;
            _configuration = configuration;
        }

        ResultDto<ProfissionalResponseDto> IProfissionalService.Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProfissionalResponseDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                 selectQuery = $@"SELECT pf.Id
                                         ,pf.PessoaId
                                         ,p.Nome
                                         ,p.NomeReduzido
                                         ,p.Cpf
                                         ,p.Sexo
                                         ,p.Email
                                         ,p.DataNascimento
                                         ,p.Ativo
                                         ,pf.RegistroProfissionalId
                                         ,pf.UFId
                                         ,pf.Numero
                                         ,pf.CNS
                                         ,pf.Ativo AS IsAtivo
                                         ,rp.Nome AS RegistroProfissional
                                         ,u.Sigla AS UFConselho
                                    FROM profissional pf
                                   INNER JOIN pessoa p ON pf.pessoaId = p.Id
                                    LEFT JOIN registroprofissional rp ON rp.id = pf.registroProfissionalId
                                    LEFT JOIN uf u ON u.Id = pf.UFId
                                   WHERE pf.Id = {id};";

                DataTable result = _profissionalRepository.Get(selectQuery);
                List<ProfissionalResponse> profissionais = result.CreateListFromTable<ProfissionalResponse>();

                if (profissionais?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = profissionais.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<ProfissionalResponse>, IEnumerable<ProfissionalResponseDto>>(profissionais ?? Enumerable.Empty<ProfissionalResponse>());
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

        ResultDto<ProfissionalResponseDto> IProfissionalService.GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProfissionalResponseDto> returnValue = new();
            string selectQuery = null;

            try
            {
                selectQuery = $@"SELECT pf.Id
                                        ,pf.pessoaId as PessoaId
                                        ,p.Nome
                                        ,p.CPF
                                        ,pf.Numero
                                        ,rp.nome AS RegistroProfissional
                                        ,u.Sigla AS UFConselho
                                        ,pf.Ativo as IsAtivo
                                        ,IFNULL(GROUP_CONCAT(esp.Nome), '') as Especialidades
                                   FROM profissional pf
                                  INNER JOIN pessoa p ON pf.pessoaId = p.Id
                                   LEFT JOIN registroProfissional rp ON pf.registroProfissionalId = rp.id
                                   LEFT JOIN uf u ON u.Id = pf.UFId
                                   LEFT JOIN profissionalespecialidade pfesp ON pf.Id = pfesp.profissionalid
                                   LEFT JOIN especialidade esp ON esp.id = pfesp.especialidadeid
                                  GROUP BY pf.Id,p.Nome,p.Cpf,rp.nome,pf.Numero,pf.Ativo
                                  ORDER BY p.Nome;";

                DataTable result = _profissionalRepository.GetAll(selectQuery);
                List<ProfissionalResponse> profissionais = result.CreateListFromTable<ProfissionalResponse>();

                if (profissionais?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = profissionais.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<ProfissionalResponse>, IEnumerable<ProfissionalResponseDto>>(profissionais ?? Enumerable.Empty<ProfissionalResponse>());
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

        public override ResultDto<ProfissionalDto> Insert(ProfissionalDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProfissionalDto> returnValue = new();
            string? insertQuery = null;
            string? insertPessoaQuery = null;

            try
            {
                PessoaDto pessoa = new()
                {
                    CPF = dto.CPF,
                    DataNascimento = dto.DataNascimento,
                    Email = dto.Email,
                    Ativo = dto.IsAtivo,
                    Nome = dto.Nome,
                    NomeReduzido = dto.NomeReduzido,
                    Sexo = dto.Sexo
                };

                ResultDto<PessoaDto> result = _pessoaService.Insert(pessoa);
                PessoaDto? pessoaFound = result.Items.FirstOrDefault();

                if (dto.Endereco is not null)
                {
                    EnderecoDto endereco = new()
                    {
                        Bairro = dto.Endereco.Bairro,
                        CEP = dto.Endereco.CEP,
                        Complemento = dto.Endereco.Complemento,
                        Logradouro = dto.Endereco.Logradouro,
                        Numero = dto.Endereco.Numero,
                        PessoaId = pessoaFound.Id
                    };

                    ResultDto<EnderecoDto> resultEndereco = _enderecoService.Insert(endereco);
                }

                if (dto.Telefone is not null && !string.IsNullOrWhiteSpace(dto.Telefone.DDDNumero))
                {
                    TelefoneDto telefone = new()
                    {
                        DDDNumero = dto.Telefone.DDDNumero,
                        TipoTelefoneId = dto.Telefone.TipoTelefoneId,
                        PessoaId = pessoaFound.Id
                    };

                    ResultDto<TelefoneDto> resultTelefone = _telefoneService.Insert(telefone);
                }

                Profissional profissional = _mapper.Map<ProfissionalDto, Profissional>(dto);
                insertQuery = $@"INSERT INTO profissional 
                                 (Id
                                 ,PessoaId
                                 ,RegistroProfissionalId
                                 ,UFId
                                 ,Numero
                                 ,CNS
                                 ,Ativo)
                                 VALUES 
                                 (null
                                 ,{pessoaFound?.Id}
                                 ,{profissional.RegistroProfissionalId}
                                 ,{profissional.UFId}
                                 ,'{profissional.Numero}'
                                 ,'{profissional.CNS}'
                                 ,true);";

                long newId = _profissionalRepository.Insert(insertQuery);
                if (newId > 0)
                    profissional.Id = (int)newId;

                var item = _mapper.Map<Profissional, ProfissionalDto>(profissional);

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

        public override ResultDto<ProfissionalDto> Update(ProfissionalDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<ProfissionalDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                PessoaDto pessoa = new()
                {
                    Id = dto.PessoaId,
                    CPF = dto.CPF,
                    DataNascimento = dto.DataNascimento,
                    Email = dto.Email,
                    Ativo = true,
                    Nome = dto.Nome,
                    NomeReduzido = dto.NomeReduzido,
                    Sexo = dto.Sexo
                };
                ResultDto<PessoaDto> result = _pessoaService.Update(pessoa);

                if (dto.Endereco is not null)
                {
                    EnderecoDto endereco = new()
                    {
                        Id = dto.Endereco.Id,
                        Bairro = dto.Endereco.Bairro,
                        CEP = dto.Endereco.CEP,
                        Complemento = dto.Endereco.Complemento,
                        Logradouro = dto.Endereco.Logradouro,
                        Numero = dto.Endereco.Numero,
                        UFId = dto.Endereco.UFId,
                        CidadeId = dto.Endereco.CidadeId,
                        PessoaId = dto.PessoaId
                    };
                    ResultDto<EnderecoDto> resultEndereco = _enderecoService.Update(endereco);
                }

                if (dto.Telefone is not null && !string.IsNullOrWhiteSpace(dto.Telefone.DDDNumero))
                {
                    TelefoneDto telefone = new()
                    {
                        DDDNumero = dto.Telefone.DDDNumero,
                        TipoTelefoneId = dto.Telefone.TipoTelefoneId,
                        PessoaId = dto.PessoaId
                    };

                    ResultDto<TelefoneDto> resultTelefone = _telefoneService.Update(telefone);
                }

                updateQuery = $@"UPDATE profissional
                                 SET RegistroProfissionalId = {dto.RegistroProfissionalId}
                                 ,UFId = {dto.UFId}
                                 ,Numero = '{dto.Numero}'
                                 ,CNS = '{dto.CNS}'
                                 ,Ativo = {dto.Ativo}
                                 WHERE Id = {dto.Id};";

                _profissionalRepository.Update(updateQuery);

                Profissional profissional = _mapper.Map<ProfissionalDto, Profissional>(dto);
                ProfissionalDto item = _mapper.Map<Profissional, ProfissionalDto>(profissional);

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
