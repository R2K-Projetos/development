using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class PacienteResponseDto : BaseDto
    {
        public int PessoaId { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string NomeReduzido { get; set; } = string.Empty;

        public string Cpf { get; set; } = string.Empty;

        public string Sexo { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime DataNascimento { get; set; }

        public bool Ativo { get; set; }

        public string? PerfilUsuario { get; set; }

        public string? StatuslUsuario { get; set; }
    }
}
