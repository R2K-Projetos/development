using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class Prontuario : BaseEntity
    {
        public int EncaminhamentoId { get; set; }
        public int ProfissionalId { get; set; }
        public int LaudoAnamneseId { get; set; }
        public DateTime DataEntrada { get; set; }
        public bool Ativo { get; set; }
    }
}
