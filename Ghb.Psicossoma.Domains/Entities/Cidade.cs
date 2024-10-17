using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class Cidade : BaseEntity
    {
        public int UFId { get; set; }
        public string Nome { get; set; } = string.Empty;
    }
}
