using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class Paciente : BaseEntity
    {
        public int PessoaId { get; set; }
        public DateTime EmissaoRG { get; set; }
        public string OrgaoRG { get; set; } = string.Empty;
        public string RG { get; set; } = string.Empty;
        public string NomeMae { get; set; } = string.Empty;
        public string NomePai { get; set; } = string.Empty;
        public string ObsPaciente { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}
