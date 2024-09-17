using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class CidadeDto : BaseDto
    {
        public string UFId { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
    }
}
