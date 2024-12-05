using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class AgendaProfissional : BaseEntity
    {
        public int ProfissionalId { get; set; }
        public int EspecialidadeId { get; set; }
        public string DiaSemana { get; set; } = string.Empty;
        public TimeSpan Hora { get; set; }
        public bool Ativo { get; set; }
    }
}
