using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class FuncionalidadePerfilUsuario : BaseEntity
    {
        public int FuncionalidadeId { get; set; }
        public int PerfilUsuarioId { get; set; }
        public bool Ativo { get; set; }
    }
}
