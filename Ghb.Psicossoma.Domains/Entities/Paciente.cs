using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class Paciente : BaseEntity
    {
        public int PessoaId { get; set; }

        public bool Ativo { get; set; }
    }
}
