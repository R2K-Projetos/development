using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class CidadeDto : BaseDto
    {
        public int UFId { get; set; }
        public string Nome { get; set; } = string.Empty;
    }
}
