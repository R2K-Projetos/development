using System.Data;
using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.SharedAbstractions.Domains.Abstractions.Base;

namespace Ghb.Psicossoma.Repositories.Abstractions
{
    public interface IEncaminhamentoRepository : IBaseRepository<Encaminhamento>
    {
        DataTable GetByIdPaciente(int id);
    }
}
