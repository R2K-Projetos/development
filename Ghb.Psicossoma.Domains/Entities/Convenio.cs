using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class Convenio : BaseEntity
    {
        public int PlanoSaudeId { get; set; }
        public int PlanoConvenioId { get; set; }
        public int ProdutoConvenioId { get; set; }
        public string Identificacao { get; set; } = string.Empty;
        public string Acomodacao { get; set; } = string.Empty;
        public string Cns { get; set; } = string.Empty;
        public string Cobertura { get; set; } = string.Empty;
        public string Empresa { get; set; } = string.Empty;
        public bool Ativo { get; set; }
}    
}
