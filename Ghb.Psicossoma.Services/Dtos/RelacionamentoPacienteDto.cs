using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class RelacionamentoPacienteDto : BaseDto
    {
        public int ResponsavelId { get; set; }
        public int DependenteId { get; set; }
        public int GrauParentescoId { get; set; }
    }
}
