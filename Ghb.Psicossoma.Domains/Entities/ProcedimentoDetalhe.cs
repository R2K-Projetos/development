using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class ProcedimentoDetalhe : BaseEntity
    {
        public int ProcedimentoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Aliquota { get; set; }
    }
}
