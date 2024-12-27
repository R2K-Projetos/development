using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Abstractions.Base;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Services.Abstractions
{
    public interface IEncaminhamentoService : IBaseService<EncaminhamentoDto>
    {
        new ResultDto<EncaminhamentoResponseDto> GetAll();
        ResultDto<EncaminhamentoDto> GetByIdPaciente(int id);
    }
}
