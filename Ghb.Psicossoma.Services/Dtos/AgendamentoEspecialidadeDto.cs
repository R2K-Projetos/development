using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class AgendamentoEspecialidadeDto : BaseDto
    {
        public int ProfissionalId { get; set; }
        public int EspecialidadeId { get; set; }
        public int PacienteId { get; set; }
        public DateTime DataAgendamento { get; set; }
        public TimeSpan Hora { get; set; }
        public bool Ativo { get; set; }
    }
}
