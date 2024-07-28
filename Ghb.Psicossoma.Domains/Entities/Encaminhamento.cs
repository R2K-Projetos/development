using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class Encaminhamento : BaseEntity
    {
        public int PacienteId { get; set; }
        public int EspecialidadeId { get; set; }
        public int ConvenioId { get; set; }
        public int CidId { get; set; }
        public int TotalSessoes { get; set; }
        public int MaximoSessoes { get; set; }
        public int QuantidadeSessoes { get; set; }
        public int SolicitacaoMedica { get; set; }
        public string Observacao { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}
