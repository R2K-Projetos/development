using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Abstractions.Base;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Services.Abstractions
{
    public interface ICidadeService : IBaseService<CidadeDto>
    {
        ResultDto<CidadeDto> GetAllByUf(int ufId);

        ResultDto<CidadeDto> GetAllByName(string name);
    }
}
