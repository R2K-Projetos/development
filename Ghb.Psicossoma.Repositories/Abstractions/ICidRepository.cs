using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.SharedAbstractions.Domains.Abstractions.Base;
using System.Data;

namespace Ghb.Psicossoma.Repositories.Abstractions
{
    public interface ICidRepository : IBaseRepository<Cid>
    {
        DataTable GetByCode(string code);
    }
}
