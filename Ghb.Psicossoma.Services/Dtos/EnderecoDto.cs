using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class EnderecoDto : BaseDto
    {
        public int PessoaId { get; set; }

        public int CidadeId { get; set; }

        public string CEP { get; set; } = string.Empty;

        public string Logradouro { get; set; } = string.Empty;

        public string Numero { get; set; } = string.Empty;

        public string Complemento { get; set; } = string.Empty;

        public string Bairro { get; set; } = string.Empty;

        public bool Ativo { get; set; }

        public int UFId { get; set; }
    }
}
