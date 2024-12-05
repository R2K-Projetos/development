using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class TelefoneResponse : BaseEntity
    {
        public int PessoaId { get; set; }
        public int TipoTelefoneId { get; set; }
        public bool Principal { get; set; }
        public string DDDNumero { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public string? TipoTelefone { get; set; } = string.Empty;
    }
}
