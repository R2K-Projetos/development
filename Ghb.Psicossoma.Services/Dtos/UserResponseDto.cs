using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos
{
    public class UserResponseDto : BaseDto
    {

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Perfil { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public bool Ativo { get; set; }
    }
}
