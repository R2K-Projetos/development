using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;
using Ghb.Psicossoma.SharedAbstractions.Services.Abstractions.Base;

namespace Ghb.Psicossoma.Services.Abstractions
{
    public interface IPacienteService : IBaseService<PacienteDto>
    {
        new ResultDto<PacienteResponseDto> Get(string id);

        new ResultDto<PacienteResponseDto> GetAll();
    }
}
