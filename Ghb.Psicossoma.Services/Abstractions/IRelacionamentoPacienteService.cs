using Ghb.Psicossoma.Services.Dtos;
using Ghb.Psicossoma.SharedAbstractions.Services.Abstractions.Base;
using Ghb.Psicossoma.SharedAbstractions.Services.Implementations;

namespace Ghb.Psicossoma.Services.Abstractions
{
    public interface IRelacionamentoPacienteService : IBaseService<RelacionamentoPacienteDto>
    {
        ResultDto<RelacionamentoPacienteDto> GetRelacionamentoPaciente(string Id);
        ResultDto<RelacionamentoPacienteDto> AdicionaRelacionamentoPaciente(RelacionamentoPacienteDto dto);
        ResultDto<RelacionamentoPacienteDto> RetiraRelacionamentoPaciente(RelacionamentoPacienteDto dto);
    }
}
