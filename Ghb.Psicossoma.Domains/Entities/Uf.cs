using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class Uf : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
    }
}
