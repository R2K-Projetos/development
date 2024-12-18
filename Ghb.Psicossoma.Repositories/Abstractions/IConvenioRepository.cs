using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.SharedAbstractions.Domains.Abstractions.Base;
using System.Data;

namespace Ghb.Psicossoma.Repositories.Abstractions
{
    public interface IConvenioRepository : IBaseRepository<Convenio>
    {
        DataTable GetSomenteAtivos();
    }
}
