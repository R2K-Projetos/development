using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class ProcedimentoDetalheDto : BaseDto
    {
        public int ProcedimentoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Aliquota { get; set; }
    }
}
