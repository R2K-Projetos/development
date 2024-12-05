using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities;

public class User : BaseEntity
{
    public int PessoaId { get; set; }

    public int PerfilUsuarioId { get; set; }

    public int StatusUsuarioId { get; set; }

    public string Senha { get; set; } = string.Empty;

    public bool Ativo { get; set; } = false;
}
