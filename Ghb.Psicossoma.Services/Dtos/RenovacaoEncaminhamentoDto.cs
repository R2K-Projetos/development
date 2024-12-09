using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class RenovacaoEncaminhamentoDto : BaseDto
    {
        public int EncaminhamentoId { get; set; }
        public DateTime DataEncaminhamento { get; set; }
        public string QuemAutorizou { get; set; } = string.Empty;
        public bool Validada { get; set; }
        public bool Ativo { get; set; }
    }
}
