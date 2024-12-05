using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class AgendamentoEspecialidade : BaseEntity
    {
        public int ProfissionalId { get; set; }
        public int EspecialidadeId { get; set; }
        public int PacienteId { get; set; }
        public DateTime DataAgendamento { get; set; }
        public TimeSpan Hora { get; set; }
        public bool Ativo { get; set; }
    }
}
