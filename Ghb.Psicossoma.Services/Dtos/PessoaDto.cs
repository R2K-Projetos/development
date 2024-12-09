using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class PessoaDto : BaseDto
    {
        public string Nome { get; set; } = string.Empty;

        public string NomeReduzido { get; set; } = string.Empty;

        public string CPF { get; set; } = string.Empty;

        public string Sexo { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime DataNascimento { get; set; }

        public bool Ativo { get; set; }

        public DateTime DataCadastro { get; set; }

        public EnderecoDto? Endereco { get; set; }

        public TelefoneDto? Telefone { get; set; }
    }
}
