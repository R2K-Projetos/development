using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Abstractions.Base;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Services.Abstractions
{
    public interface IAgendaProfissionalService : IBaseService<AgendaProfissionalDto>
    {
        ResultDto<AgendaProfissionalDto> GetByProfissional(string ProfissionalId);
    }
}
