using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class Procedimento : BaseEntity
    {
        public int EspecialidadeId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal ValorBase { get; set; }
    }
}
