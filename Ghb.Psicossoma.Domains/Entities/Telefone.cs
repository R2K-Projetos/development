using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class Telefone : BaseEntity
    {
        public int PessoaId { get; set; }
        public int TipoTelefoneId { get; set; }
        public string DDDNumero { get; set; } = string.Empty;
        public bool Principal { get; set; }
        public bool Ativo { get; set; }
    }
}
