using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class ProfissionalResponse : BaseEntity
    {
        public int PessoaId { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string NomeReduzido { get; set; } = string.Empty;

        public string Cpf { get; set; } = string.Empty;

        public string Sexo { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime DataNascimento { get; set; }

        public bool IsAtivo { get; set; } = false;

        public string Numero { get; set; } = string.Empty;

        public string RegistroProfissional { get; set; } = string.Empty;

        public string Especialidades { get; set; } = string.Empty;
    }
}
