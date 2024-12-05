using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class GuiaAutorizacao : BaseEntity
    {
        public int EncaminhamentoId { get; set; }
        public int GrupoGuiaId { get; set; }
        public string Numero { get; set; } = string.Empty;
        public DateTime DataEmissao { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
        public int TotalSessoes { get; set; }
        public bool Ativo { get; set; }
    }
}
