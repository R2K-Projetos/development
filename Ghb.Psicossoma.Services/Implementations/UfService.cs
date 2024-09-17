using AutoMapper;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ghb.Psicossoma.Services.Implementations
{
    public class UfService : IUfService
    {
        private readonly IUfRepository _ufRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UfService> _logger;

        public UfService(IUfRepository ufRepository,
                             ILogger<UfService> logger,
                             IMapper mapper,
                             IConfiguration configuration)
        {
            _ufRepository = ufRepository;
            _configuration = configuration;
            _logger = logger;
        }


    }
}
