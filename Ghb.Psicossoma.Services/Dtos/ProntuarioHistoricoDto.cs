using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class ProntuarioHistoricoDto : BaseDto
    {
        public int ProntuarioId { get; set; }
        public int ProfissionalId { get; set; }
        public string DescricaoGeral { get; set; } = string.Empty;
        public string DescricaoReservada { get; set; } = string.Empty;
        public DateTime DataHistorico { get; set; }
        public bool Ativo { get; set; }
    }
}
