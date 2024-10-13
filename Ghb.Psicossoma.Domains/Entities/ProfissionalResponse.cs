using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class ProfissionalResponse : BaseEntity
    {
        public int PessoaId { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Cpf { get; set; }

        public string Numero { get; set; } = string.Empty;

        public string DddNumero { get; set; } = string.Empty;

        public string RegistroProfissional { get; set; } = string.Empty;

        public string Especialidade { get; set; } = string.Empty;

        public bool Ativo { get; set; } = false;
    }
}
