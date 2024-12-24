﻿using AutoMapper;
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
    public class PacienteService : BaseService<PacienteDto, Paciente>, IPacienteService
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IPessoaService _pessoaService;
        private readonly IEnderecoService _enderecoService;
        private readonly ITelefoneService _telefoneService;
        private readonly ILogger<PacienteService> _logger;

        public PacienteService(IPacienteRepository pacienteRepository,
                               IPessoaService pessoaService,
                               IEnderecoService enderecoService,
                               ITelefoneService telefoneService,
                               ILogger<PacienteService> logger,
                               IMapper mapper) : base(pacienteRepository, mapper)
        {
            _pacienteRepository = pacienteRepository;
            _pessoaService = pessoaService;
            _enderecoService = enderecoService;
            _telefoneService = telefoneService;
            _logger = logger;
        }

        ResultDto<PacienteResponseDto> IPacienteService.Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PacienteResponseDto> returnValue = new();

            try
            {
                string selectQuery = $@"select pc.Id                                               
                                               ,pc.PessoaId
                                               ,p.Nome
                                               ,p.NomeReduzido
                                               ,p.CPF
                                               ,p.Sexo
                                               ,p.Email
                                               ,p.DataNascimento
                                               ,pc.EmissaoRG
                                               ,pc.OrgaoRG
                                               ,pc.RG
                                               ,pc.NomeMae
                                               ,pc.NomePai
                                               ,pc.ObsPaciente
                                               ,pc.Ativo as IsAtivo
                                               ,pu.Nome as PerfilUsuario
                                               ,st.Nome as StatuslUsuario
                                          from paciente pc
                                         INNER JOIN pessoa p on p.Id = pc.pessoaId
                                          LEFT JOIN usuario u on u.PessoaId = p.Id
                                          LEFT JOIN perfilusuario pu on pu.Id = u.PerfilUsuarioId
                                          LEFT JOIN statususuario st on st.Id = u.StatusUsuarioId
                                         WHERE pc.Id = {id};";

                DataTable result = _pacienteRepository.Get(selectQuery);
                List<PacienteResponse> users = result.CreateListFromTable<PacienteResponse>();

                if (users?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = users.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<PacienteResponse>, IEnumerable<PacienteResponseDto>>(users ?? Enumerable.Empty<PacienteResponse>());
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

        ResultDto<PacienteResponseDto> IPacienteService.GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PacienteResponseDto> returnValue = new();

            try
            {
                string selectQuery = $@"select pc.Id                                               
                                               ,pc.PessoaId
                                               ,p.Nome
                                               ,p.NomeReduzido
                                               ,p.CPF
                                               ,p.Sexo
                                               ,p.Email
                                               ,p.DataNascimento
                                               ,pc.EmissaoRG
                                               ,pc.OrgaoRG
                                               ,pc.RG
                                               ,pc.NomeMae
                                               ,pc.NomePai
                                               ,pc.ObsPaciente
                                               ,pc.Ativo as IsAtivo
                                          from paciente pc
                                         INNER JOIN pessoa p on p.Id = pc.pessoaId
                                          LEFT JOIN usuario u on u.PessoaId = p.Id
                                          LEFT JOIN perfilusuario pu on pu.Id = u.PerfilUsuarioId
                                          LEFT JOIN statususuario st on st.Id = u.StatusUsuarioId
                                         where 1 = 1
                                         order by p.Nome;";

                DataTable result = _pacienteRepository.GetAll(selectQuery);
                List<PacienteResponse> pacientes = result.CreateListFromTable<PacienteResponse>();

                if (pacientes?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = pacientes.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<PacienteResponse>, IEnumerable<PacienteResponseDto>>(pacientes ?? Enumerable.Empty<PacienteResponse>());
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

        public override ResultDto<PacienteDto> Insert(PacienteDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PacienteDto> returnValue = new();
            string? insertQuery = null;
            string? insertPessoaQuery = null;

            try
            {
                PessoaDto pessoa = new()
                {
                    CPF = dto.CPF,
                    DataNascimento = dto.DataNascimento,
                    Email = dto.Email,
                    Ativo = true,
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
                        UFId = dto.Endereco.UFId,
                        CidadeId = dto.Endereco.CidadeId,
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

                Paciente paciente = _mapper.Map<PacienteDto, Paciente>(dto);
                insertQuery = $@"INSERT INTO paciente 
                                 (Id, PessoaId, EmissaoRG, OrgaoRG, RG, NomeMae, NomePai, ObsPaciente, Ativo)
                                 VALUES 
                                 (null 
                                 ,{pessoaFound?.Id} 
                                 ,'{dto.EmissaoRG}'
                                 ,'{dto.OrgaoRG}'
                                 ,'{dto.RG}'
                                 ,'{dto.NomeMae}'
                                 ,'{dto.NomePai}'
                                 ,'{dto.ObsPaciente}'
                                 ,true);";

                long newId = _pacienteRepository.Insert(insertQuery);
                if (newId > 0)
                    paciente.Id = (int)newId;

                var item = _mapper.Map<Paciente, PacienteDto>(paciente);

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

        public override ResultDto<PacienteDto> Update(PacienteDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PacienteDto> returnValue = new();
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

                var entidade = _mapper.Map<PacienteDto, Paciente>(dto);
                updateQuery = $@"UPDATE paciente 
                                 SET EmissaoRG = '{entidade.EmissaoRG}' 
                                 ,OrgaoRG = '{entidade.OrgaoRG}' 
                                 ,RG = '{entidade.RG}' 
                                 ,NomeMae = '{entidade.NomeMae}' 
                                 ,NomePai = '{entidade.NomePai}' 
                                 ,ObsPaciente = '{entidade.ObsPaciente}' 
                                 ,Ativo = {entidade.Ativo}
                                 WHERE id = {entidade.Id};";

                _pacienteRepository.Update(updateQuery);
                var item = _mapper.Map<Paciente, PacienteDto>(entidade);

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
