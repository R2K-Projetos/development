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
    public class RegistroProfissionalService : BaseService<RegistroProfissionalDto, RegistroProfissional>, IRegistroProfissionalService
    {
        private readonly IRegistroProfissionalRepository _registroProfissionalRepository;
        private readonly IConfiguration _configuration;

        public RegistroProfissionalService(IRegistroProfissionalRepository registroProfissionalRepository, IMapper mapper, IConfiguration configuration) : base(registroProfissionalRepository, mapper)
        {
            _registroProfissionalRepository = registroProfissionalRepository;
            _configuration = configuration;
        }

        public override ResultDto<RegistroProfissionalDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<RegistroProfissionalDto> returnValue = new();

            try
            {
                string selectQuery = $@"SELECT Id, Descricao FROM registroProfissional;";

                DataTable result = _registroProfissionalRepository.GetAll(selectQuery);
                List<RegistroProfissional> status = result.CreateListFromTable<RegistroProfissional>();

                if (status?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = status.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<RegistroProfissional>, IEnumerable<RegistroProfissionalDto>>(status ?? Enumerable.Empty<RegistroProfissional>());
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
            }

            elapsedTime.Stop();
            returnValue.ElapsedTime = elapsedTime.Elapsed;

            return returnValue;
        }

    }
}
