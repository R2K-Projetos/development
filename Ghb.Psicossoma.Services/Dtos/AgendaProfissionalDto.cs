using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class AgendaProfissionalDto : BaseDto
    {
        public int ProfissionalId { get; set; }
        public int EspecialidadeId { get; set; }
        public string DiaSemana { get; set; } = string.Empty;
        public TimeSpan Hora { get; set; }
        public bool Ativo { get; set; }
    }
}
