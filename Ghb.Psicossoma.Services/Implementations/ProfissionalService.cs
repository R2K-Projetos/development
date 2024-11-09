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
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProfissionalService> _logger;

        public ProfissionalService(IProfissionalRepository profissionalRepository,
                                   IPessoaService pessoaService,
                                   IEnderecoService enderecoService,
                                   ILogger<ProfissionalService> logger,
                                   IMapper mapper,
                                   IConfiguration configuration) : base(profissionalRepository, mapper)
        {
            _logger = logger;
            _profissionalRepository = profissionalRepository;
            _pessoaService = pessoaService;
            _enderecoService = enderecoService;
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
                 selectQuery = $@"SELECT pf.Id, ps.Nome, rp.Descricao AS RegistroProfissional, pf.Numero, pf.Ativo
                                  FROM profissional pf
                                  INNER JOIN pessoa ps ON pf.pessoaId = ps.Id
                                  INNER JOIN registroProfissional rp ON pf.registroProfissionalId = rp.id
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
                                        ,ps.Nome
                                        ,ps.Cpf
                                        ,pf.Numero
                                        ,rp.nome AS RegistroProfissional
                                        ,pf.Ativo
                                        ,IFNULL(GROUP_CONCAT(esp.Nome), '') as Especialidade
                                   FROM profissional pf
                                  INNER JOIN pessoa ps ON pf.pessoaId = ps.Id
                                   LEFT JOIN registroProfissional rp ON pf.registroProfissionalId = rp.id
                                   LEFT JOIN profissionalespecialidade pfesp ON pf.Id = pfesp.profissionalid
                                   LEFT JOIN especialidade esp ON esp.id = pfesp.especialidadeid
                                  GROUP BY pf.Id,ps.Nome,ps.Cpf,rp.nome,pf.Numero,pf.Ativo
                                  ORDER BY ps.Nome;";

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
                    Cpf = dto.Cpf,
                    DataNascimento = dto.DataNascimento,
                    Email = dto.Email,
                    Ativo = dto.IsAtivo,
                    Nome = dto.Nome,
                    NomeReduzido = dto.NomeReduzido,
                    Sexo = dto.Sexo
                };


                ResultDto<PessoaDto> result = _pessoaService.Insert(pessoa);
                PessoaDto? pessoaFound = result.Items.FirstOrDefault();

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

                Profissional profissional = _mapper.Map<ProfissionalDto, Profissional>(dto);
                insertQuery = $@"INSERT INTO profissional(Id, PessoaId, RegistroProfissionalId, Numero, Ativo)
                                 VALUES(null, {pessoaFound?.Id}, {profissional.RegistroProfissionalId}, '{profissional.Numero}', {profissional.Ativo});";

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
    }
}
