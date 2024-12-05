using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class Log : BaseEntity
    {
        public int TabelaId { get; set; }
        public int UsuarioId { get; set; }
        public string Tabela { get; set; } = string.Empty;
        public string TipoAcao { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
    }
}
