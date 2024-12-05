using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class ConsultaDto : BaseDto
    {
        public int AgendaProfissionalId { get; set; }
        public int PacienteId { get; set; }
        public int EncaminhamentoId { get; set; }
        public int ProcedimentoDetalheId { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
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
