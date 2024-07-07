using Ghb.Psicossoma.SharedAbstractions.Domains.Abstractions.Base;

namespace Ghb.Psicossoma.SharedAbstractions.Domains.Entities;

public class BaseEntity : IBaseEntity
{
    public long Id { get; set; }
}
