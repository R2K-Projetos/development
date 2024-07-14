using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class Endereco : BaseEntity
    {
        public int PessoaId { get; set; }

        public int CidadeId { get; set; }

        public string Cep { get; set; } = string.Empty;

        public string Logradouro { get; set; } = string.Empty;

        public string Numero { get; set; } = string.Empty;

        public string Complemento { get; set; } = string.Empty;

        public string Bairro { get; set; } = string.Empty;

        public bool Ativo { get; set; }
    }
}
