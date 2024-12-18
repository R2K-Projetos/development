using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class LaudoAnamnese : BaseEntity
    {
        public int EspecialidadeId { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}
