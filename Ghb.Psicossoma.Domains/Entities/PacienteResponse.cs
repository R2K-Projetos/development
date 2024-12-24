using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class PacienteResponse : BaseEntity
    {
        public int PessoaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string NomeReduzido { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Sexo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public DateTime EmissaoRG { get; set; }
        public string OrgaoRG { get; set; } = string.Empty;
        public string RG { get; set; } = string.Empty;
        public string NomeMae { get; set; } = string.Empty;
        public string NomePai { get; set; } = string.Empty;
        public string ObsPaciente { get; set; } = string.Empty;
        public bool IsAtivo { get; set; }

        public string? PerfilUsuario { get; set; }
        public string? StatuslUsuario { get; set; }
    }
}
