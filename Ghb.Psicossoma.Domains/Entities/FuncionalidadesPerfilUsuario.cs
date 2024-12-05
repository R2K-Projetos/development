using Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

namespace Ghb.Psicossoma.Domains.Entities
{
    public class FuncionalidadesPerfilUsuario : BaseEntity
    {
        public int FuncionalidadesId { get; set; }
        public int PerfilUsuarioId { get; set; }
        public bool Ativo { get; set; }
    }
}
