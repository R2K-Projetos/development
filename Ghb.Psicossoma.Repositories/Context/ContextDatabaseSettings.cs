using Ghb.Psicossoma.SharedAbstractions.Repositories.Abstractions;

namespace Ghb.Psicossoma.Repositories.Context
{
    public class ContextDatabaseSettings : IContextDatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
    }
}
