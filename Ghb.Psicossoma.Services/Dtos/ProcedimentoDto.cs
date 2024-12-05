using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class ProcedimentoDto : BaseDto
    {
        public int EspecialidadeId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal ValorBase { get; set; }
    }
}
