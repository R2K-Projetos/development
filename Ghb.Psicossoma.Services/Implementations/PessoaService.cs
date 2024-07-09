using AutoMapper;
using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Library.Extensions;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.Repositories.Implementations;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
