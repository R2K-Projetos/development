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
    public class PerfilUsuarioService : BaseService<PerfilUsuarioDto, PerfilUsuario>, IPerfilUsuarioService
    {
        private readonly IPerfilUsuarioRepository _perfilUsuarioRepository;
        private readonly IConfiguration _configuration;

        public PerfilUsuarioService(IPerfilUsuarioRepository perfilUsuarioRepository, IMapper mapper, IConfiguration configuration) : base(perfilUsuarioRepository, mapper)
        {
            _perfilUsuarioRepository = perfilUsuarioRepository;
            _configuration = configuration;
        }

        public override ResultDto<PerfilUsuarioDto> GetAll()
        {
            Stopwatch elapsedTime = new();
            elapsedTime.Start();

            ResultDto<PerfilUsuarioDto> returnValue = new();

            try
            {
                string selectQuery = $@"SELECT Id, Descricao FROM perfilUsuario;";

                DataTable result = _perfilUsuarioRepository.GetAll(selectQuery);
                List<PerfilUsuario> status = result.CreateListFromTable<PerfilUsuario>();

                if (status?.Count > 0)
                {
                    returnValue.CurrentPage = 1;
                    returnValue.PageSize = -1;
                    returnValue.TotalItems = status.Count;
                    returnValue.Items = _mapper.Map<IEnumerable<PerfilUsuario>, IEnumerable<PerfilUsuarioDto>>(status ?? Enumerable.Empty<PerfilUsuario>());
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
