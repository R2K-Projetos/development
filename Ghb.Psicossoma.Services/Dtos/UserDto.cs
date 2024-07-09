using Ghb.Psicossoma.SharedAbstractions.Services.Dtos.Base;

namespace Ghb.Psicossoma.Services.Dtos;

public class UserDto : BaseDto
{
    public long PessoaId { get; set; }

    public int PerfilUsuarioId { get; set; }

    public int StatusId { get; set; }

    public string Senha { get; set; } = string.Empty;

    public bool Ativo { get; set; } = false;
}
