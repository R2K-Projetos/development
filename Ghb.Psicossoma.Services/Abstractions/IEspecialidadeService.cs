using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Abstractions.Base;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Services.Abstractions
{
    public interface IEspecialidadeService : IBaseService<EspecialidadeDto>
    {
        ResultDto<EspecialidadeResponseDto> GetEspecialidadeDisponivel(string ProfissionalId);

        ResultDto<ProfissionalEspecialidadeDto> AdicionaEspecialidade(ProfissionalEspecialidadeDto dto);
    }
}
