using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos;

public class UserDto : BaseDto
{
    public long IdPessoa { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;

    public string Nome { get; set; } = string.Empty;

    public string Perfil { get; set; } = string.Empty;
}
