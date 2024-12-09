using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class Arquivo : BaseEntity
    {
        public int TipoArquivoId { get; set; }
        public int IdTabela { get; set; }
        public string Tabela { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}
