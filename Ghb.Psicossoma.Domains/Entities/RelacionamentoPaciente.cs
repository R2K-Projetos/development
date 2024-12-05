using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class RelacionamentoPaciente : BaseEntity
    {
        public int ResponsavelId { get; set; }
        public int DependenteId { get; set; }
        public int GrauParentescoId { get; set; }
    }
}
