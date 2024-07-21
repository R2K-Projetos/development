using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class ConvenioDto : BaseDto
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
