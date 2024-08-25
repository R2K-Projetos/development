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
    public class PessoaService : BaseService<PessoaDto, Pessoa>, IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PessoaService> _logger;

        public PessoaService(IPessoaRepository pessoaRepository,
                             ILogger<PessoaService> logger,
                             IMapper mapper,
                             IConfiguration configuration) : base(pessoaRepository, mapper)
        {
            _pessoaRepository = pessoaRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<PessoaDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PessoaDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, Nome, NomeReduzido, Cpf, Sexo, Email, DataNascimento, Ativo
                                 FROM pessoa
                                 WHERE id = {id};";

                DataTable result = _pessoaRepository.Get(selectQuery);
                List<Pessoa> pessoas = result.CreateListFromTable<Pessoa>();

                if (pessoas?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = pessoas.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Pessoa>, IEnumerable<PessoaDto>>(pessoas ?? Enumerable.Empty<Pessoa>());
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

        public override ResultDto<PessoaDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PessoaDto> returnValue = new();
            string selectQuery = null;

            try
            {
                selectQuery = $@"SELECT Id, Nome, NomeReduzido, Cpf, Sexo, Email, DataNascimento, Ativo
                                 FROM pessoa;";

                DataTable result = _pessoaRepository.GetAll(selectQuery);
                List<Pessoa> pessoas = result.CreateListFromTable<Pessoa>();

                if(pessoas?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = pessoas.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Pessoa>, IEnumerable<PessoaDto>>(pessoas ?? Enumerable.Empty<Pessoa>());
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

        public override ResultDto<PessoaDto> Insert(PessoaDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PessoaDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var pessoa = _mapper.Map<PessoaDto, Pessoa>(dto);
                insertQuery = $@"INSERT INTO pessoa(Id, Nome, NomeReduzido, CPF, Sexo, Email, DataNascimento, Ativo)
                                 VALUES(null, '{pessoa.Nome}', '{pessoa.NomeReduzido}', '{pessoa.Cpf}', '{pessoa.Sexo}', '{pessoa.Email.ToLower()}', '{pessoa.DataNascimento:yyyy-MM-dd}', {pessoa.Ativo});";

                long newId = _pessoaRepository.Insert(insertQuery);
                if (newId > 0)
                    pessoa.Id = (int)newId;

                var item = _mapper.Map<Pessoa, PessoaDto>(pessoa);

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

        public override ResultDto<PessoaDto> Update(PessoaDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PessoaDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var pessoa = _mapper.Map<PessoaDto, Pessoa>(dto);
                updateQuery = $@"UPDATE pessoa 
                                 SET Nome = '{pessoa.Nome}', NomeReduzido = '{pessoa.NomeReduzido}',CPF = '{pessoa.Cpf}', Sexo = '{pessoa.Sexo}', Email = '{pessoa.Email.ToLower()}', DataNascimento = '{pessoa.DataNascimento:yyyy-MM-dd}', Ativo = {pessoa.Ativo}
                                 WHERE id = {pessoa.Id};";

                _pessoaRepository.Update(updateQuery);
                var item = _mapper.Map<Pessoa, PessoaDto>(pessoa);

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
