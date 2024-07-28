using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class Prontuario : BaseEntity
    {
        public int EncaminhamentoId { get; set; }
        public int ProfissionalId { get; set; }
        public int PacienteId { get; set; }
        public string DescricaoGeral { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public bool Ativo { get; set; }
    }
}
