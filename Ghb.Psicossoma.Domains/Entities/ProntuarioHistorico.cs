using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class ProntuarioHistorico : BaseEntity
    {
        public int ProntuarioId { get; set; }
        public int ProfissionalId { get; set; }
        public string DescricaoGeral { get; set; } = string.Empty;
        public string DescricaoReservada { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public bool Ativo { get; set; }
    }
}
