using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class ArquivoDto : BaseDto
    {
        public int TipoArquivoId { get; set; }
        public int IdTabela { get; set; }
        public string Tabela { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}
