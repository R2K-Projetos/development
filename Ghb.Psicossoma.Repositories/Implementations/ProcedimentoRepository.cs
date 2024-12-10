using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Repositories.Implementations.Base;

namespace Ghb.Psicossoma.Repositories.Implementations
{
    public class ProcedimentoRepository : BaseRepository<Procedimento>, IProcedimentoRepository
    {
        public ProcedimentoRepository(IContextDatabaseSettings settings) : base(settings)
        {
            
        }
    }
}
