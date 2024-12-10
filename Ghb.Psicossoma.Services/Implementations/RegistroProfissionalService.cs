using AutoMapper;
using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Library.Extensions;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.Repositories.Implementations;
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
    public class RegistroProfissionalService : BaseService<RegistroProfissionalDto, RegistroProfissional>, IRegistroProfissionalService
    {
        private readonly IRegistroProfissionalRepository _registroProfissionalRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RegistroProfissionalService> _logger;

        public RegistroProfissionalService(IRegistroProfissionalRepository registroProfissionalRepository,
                                           ILogger<RegistroProfissionalService> logger,
                                           IMapper mapper,
                                           IConfiguration configuration) : base(registroProfissionalRepository, mapper)
        {
            _registroProfissionalRepository = registroProfissionalRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public override ResultDto<RegistroProfissionalDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<RegistroProfissionalDto> returnValue = new();

            try
            {
                string selectQuery = $@"SELECT Id, Nome FROM registroProfissional;";

                DataTable result = _registroProfissionalRepository.GetAll(selectQuery);
                List<RegistroProfissional> list = result.CreateListFromTable<RegistroProfissional>();

                if (list?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = list.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<RegistroProfissional>, IEnumerable<RegistroProfissionalDto>>(list ?? Enumerable.Empty<RegistroProfissional>());
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

        public override ResultDto<RegistroProfissionalDto> Get(string id)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<RegistroProfissionalDto> returnValue = new();
            string? selectQuery = null;

            try
            {
                selectQuery = $@"select Id
                                        ,Nome
                                   FROM registroprofissional
                                  WHERE Id = {id};";

                DataTable result = _registroProfissionalRepository.Get(selectQuery);
                List<RegistroProfissional> item = result.CreateListFromTable<RegistroProfissional>();

                if (item?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = item.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<RegistroProfissional>, IEnumerable<RegistroProfissionalDto>>(item ?? Enumerable.Empty<RegistroProfissional>());
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

        public override ResultDto<RegistroProfissionalDto> Insert(RegistroProfissionalDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<RegistroProfissionalDto> returnValue = new();
            string? insertQuery = null;

            try
            {
                var entidade = _mapper.Map<RegistroProfissionalDto, RegistroProfissional>(dto);
                insertQuery = $@"INSERT INTO registroprofissional 
                                 (Nome)
                                 VALUES 
                                 ('{entidade.Nome}');";

                long newId = _registroProfissionalRepository.Insert(insertQuery);
                if (newId > 0)
                    entidade.Id = (int)newId;

                var item = _mapper.Map<RegistroProfissional, RegistroProfissionalDto>(entidade);

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

        public override ResultDto<RegistroProfissionalDto> Update(RegistroProfissionalDto dto)
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<RegistroProfissionalDto> returnValue = new();
            string? updateQuery = null;

            try
            {
                var entidade = _mapper.Map<RegistroProfissionalDto, RegistroProfissional>(dto);
                updateQuery = $@"UPDATE registroprofissional 
                                 SET Nome = '{entidade.Nome}'
                                 WHERE id = {entidade.Id};";

                _registroProfissionalRepository.Update(updateQuery);
                var item = _mapper.Map<RegistroProfissional, RegistroProfissionalDto>(entidade);

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
