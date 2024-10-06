using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class UfDto : BaseDto
    {
        public string Nome { get; set; } = string.Empty;

        public string Sigla { get; set; } = string.Empty;
    }
}
