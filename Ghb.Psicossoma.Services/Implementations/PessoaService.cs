using AutoMapper;
using System.Data;
using System.Diagnostics;
using Ghb.Psicossoma.Services.Dtos;
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

        public PessoaService(IPessoaRepository pessoaRepository, IMapper mapper, IConfiguration configuration) : base(pessoaRepository, mapper)
        {
            _pessoaRepository = pessoaRepository;
            _configuration = configuration;
        }

        public override ResultDto<PessoaDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            var returnValue = new ResultDto<PessoaDto>();

            try
            {
                string selectQuery = $@"SELECT id, nome, nomeReduzido, cpf, sexo, email, datanascimento, registroativo
                                        FROM pessoa
                                        WHERE id = {id};";

                DataTable result = _pessoaRepository.Get(selectQuery);

                if (result?.Rows.Count > 0)
                {
                    List<Pessoa> pessoaList = new List<Pessoa>();
                    pessoaList = (from DataRow dr in result.Rows
                                  select new Pessoa()
                                  {
                                      Id = Convert.ToInt32(dr["id"]),
                                      Cpf = dr["cpf"].ToString(),
                                      DataNascimento = Convert.ToDateTime(dr["datanascimento"]),
                                      Email = dr["email"].ToString(),
                                      Nome = dr["nome"].ToString(),
                                      NomeReduzido = dr["nomereduzido"].ToString(),
                                      RegistroAtivo = Convert.ToBoolean(dr["registroativo"]),
                                      Sexo = dr["sexo"].ToString()
                                  }).ToList();

                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = pessoaList.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Pessoa>, IEnumerable<PessoaDto>>(pessoaList ?? Enumerable.Empty<Pessoa>());
                    returnValue.WasExecuted = true;
                    returnValue.ResponseCode = 200;
                }
                else
                {
                    returnValue.BindError(404, $"{_entityName.ToUpper()}_EMPTY" ?? "Não foram encontrados dados para exibição");
                }
            }
            catch (Exception ex)
            {
                returnValue.BindError(500, ex.GetErrorMessage());
            }

            elapsedTime.Stop();
            returnValue.ElapsedTime = elapsedTime.Elapsed;

            return returnValue;
        }

        public override ResultDto<PessoaDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            var returnValue = new ResultDto<PessoaDto>();

            try
            {
                string selectQuery = $@"SELECT id, nome, nomeReduzido, cpf, sexo, email, datanascimento, registroativo
                                        FROM pessoa;";

                DataTable result = _pessoaRepository.GetAll(selectQuery);

                if (result?.Rows.Count > 0)
                {
                    List<Pessoa> pessoaList = new List<Pessoa>();
                    pessoaList = (from DataRow dr in result.Rows
                                   select new Pessoa()
                                   {
                                       Id = Convert.ToInt32(dr["id"]),
                                       Cpf = dr["cpf"].ToString(),
                                       DataNascimento = Convert.ToDateTime(dr["datanascimento"]),
                                       Email = dr["email"].ToString(),
                                       Nome = dr["nome"].ToString(),
                                       NomeReduzido = dr["nomereduzido"].ToString(),
                                       RegistroAtivo = Convert.ToBoolean(dr["registroativo"]),
                                       Sexo = dr["sexo"].ToString()
                                   }).ToList();

                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = pessoaList.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<Pessoa>, IEnumerable<PessoaDto>>(pessoaList ?? Enumerable.Empty<Pessoa>());
                    returnValue.WasExecuted = true;
                    returnValue.ResponseCode = 200;
                }
                else
                {
                    returnValue.BindError(404, $"{_entityName.ToUpper()}_EMPTY" ?? "Não foram encontrados dados para exibição");
                }
            }
            catch (Exception ex)
            {
                returnValue.BindError(500, ex.GetErrorMessage());
            }

            elapsedTime.Stop();
            returnValue.ElapsedTime = elapsedTime.Elapsed;

            return returnValue;
        }

        public override ResultDto<PessoaDto> Insert(PessoaDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            var returnValue = new ResultDto<PessoaDto>();

            try
            {
                var pessoa = _mapper.Map<PessoaDto, Pessoa>(dto);
                string insertQuery = $@"INSERT INTO pessoa(id, nome, nomeReduzido, cpf, sexo, email, datanascimento, registroativo)
                                        VALUES(null, {pessoa.Nome}, '{pessoa.NomeReduzido}', '{pessoa.Cpf}', '{pessoa.Sexo}', '{pessoa.Email.ToLower()}', '{pessoa.DataNascimento:yyyy-MM-dd}', {pessoa.RegistroAtivo});";

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
            }

            elapsedTime.Stop();
            returnValue.ElapsedTime = elapsedTime.Elapsed;

            return returnValue;
        }
    }
}
