﻿using Ghb.Psicossoma.Domains.Entities;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Repositories.Abstractions;
using Ghb.Psicossoma.SharedAbstractions.Repositories.Implementations.Base;

namespace Ghb.Psicossoma.Repositories.Implementations
{
    public class LaudoAnamneseRepository : BaseRepository<LaudoAnamnese>, ILaudoAnamneseRepository
    {
        public LaudoAnamneseRepository(IContextDatabaseSettings settings) : base(settings)
        {
            
        }
    }
}
