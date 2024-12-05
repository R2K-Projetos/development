using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class RenovacaoEncaminhamento : BaseEntity
    {
        public int EncaminhamentoId { get; set; }
        public DateTime DataEncaminhamento { get; set; }
        public string QuemAutorizou { get; set; } = string.Empty;
        public bool Validada { get; set; }
        public bool Ativo { get; set; }
    }
}
