using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class EncaminhamentoDto : BaseDto
    {
        public int PacienteId { get; set; }
        public int EspecialidadeId { get; set; }
        public int PlanoSaudeId { get; set; }
        public int CidId { get; set; }
        public int EncaminhamentoOrigemId { get; set; }
        public int TotalSessoes { get; set; }
        public int MaximoSessoes { get; set; }
        public int SessoesRealizadas { get; set; }
        public bool SolicitacaoMedica { get; set; }
        public string Observacao { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}
