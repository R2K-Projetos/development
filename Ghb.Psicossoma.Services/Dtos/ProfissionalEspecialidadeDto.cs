using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class ProfissionalEspecialidadeDto : BaseDto
    {
        public int ProfissionalId { get; set; }
        public int EspecialidadeId { get; set; }
        public bool Ativo { get; set; }
    }
}
