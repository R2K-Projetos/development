using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class Cidade : BaseEntity
    {
        public string UFId { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
    }
}
