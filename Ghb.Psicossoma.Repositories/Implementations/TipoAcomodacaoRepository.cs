using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Repositories.Implementations.Base;

namespace Ghb.Psicossoma.Repositories.Implementations
{
    public class TipoAcomodacaoRepository : BaseRepository<TipoAcomodacao>, ITipoAcomodacaoRepository
    {
        public TipoAcomodacaoRepository(IContextDatabaseSettings settings) : base(settings)
        {

        }
    }
}
