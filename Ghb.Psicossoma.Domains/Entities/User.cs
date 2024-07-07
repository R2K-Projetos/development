using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities;

public class User : BaseEntity
{
    public int IdPessoa { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Senha { get; set; } = string.Empty;

    public string Nome { get; set; } = string.Empty;

    public string Perfil { get; set; } = string.Empty;
}
