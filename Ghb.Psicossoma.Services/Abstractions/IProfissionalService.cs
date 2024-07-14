using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Abstractions.Base;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Services.Abstractions
{
    public interface IProfissionalService : IBaseService<ProfissionalDto>
    {
        new ResultDto<ProfissionalResponseDto> Get(string id);

        new ResultDto<ProfissionalResponseDto> GetAll();
    }
}
