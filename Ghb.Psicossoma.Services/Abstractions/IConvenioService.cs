using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Abstractions.Base;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Services.Abstractions
{
    public interface IConvenioService : IBaseService<ConvenioDto>
    {
        new ResultDto<ConvenioResponseDto> Get(string id);
        new ResultDto<ConvenioResponseDto> GetAll();
    }
}
