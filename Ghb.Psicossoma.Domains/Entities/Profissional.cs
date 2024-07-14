using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class Profissional : BaseEntity
    {
        public int PessoaId { get; set; }

        public int RegistroProfissionalId { get; set; }

        public string Numero { get; set; } = string.Empty;

        public bool Ativo { get; set; } = false;
    }
}
