using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class EspecialidadeResponseDto : BaseDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Checked { get; set; } = string.Empty;
    }
}
