using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class Consulta : BaseEntity
    {
        public int ProfissionalId { get; set; }
        public int EncaminhamentoId { get; set; }
        public int ProcedimentoDetalheId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataConsulta { get; set; }
        public TimeSpan Hora { get; set; }
        public bool PacienteAvisado { get; set; }
        public bool ProfissionalAvisado { get; set; }
        public bool PacienteCompareceu { get; set; }
        public bool ProfissionalCompareceu { get; set; }
        public bool PacienteDesmarcou { get; set; }
        public bool ProfissionalDesmarcou { get; set; }
        public bool Ativo { get; set; }
    }
}
