using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class ProfissionalEspecialidade : BaseEntity
    {
        public int ProfissionalId { get; set; }
        public int EspecialidadeId { get; set; }
        public bool Ativo {  get; set; }
    }
}
