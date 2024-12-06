using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class PlanoSaudeDto : BaseDto
    {
        public int ConvenioId { get; set; }
        public int TipoAcomodacaoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string ProdutoPlano { get; set; } = string.Empty;
        public bool Acompanhante { get; set; }
        public string CodIdent { get; set; } = string.Empty;
        public string CNS { get; set; } = string.Empty;
        public string Cobertura { get; set; } = string.Empty;
        public string Empresa { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}
