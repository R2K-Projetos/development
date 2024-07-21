using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Ghb.Psicossoma.Repositories.Context;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.Repositories.Implementations;
using Ghb.Psicossoma.Services.MapperConfiguration;
using Ghb.Psicossoma.SharedAbstractions.Repositories.Abstractions;


namespace Ghb.Psicossoma.Crosscutting.IoC;

public static class ContainerIoC
{
    static IConfigurationSection? _contextDatabaseSetting;

    public static void InitializeContainerIoC(this IServiceCollection services, IConfigurationSection contextDatabaseSetting)
    {
        _contextDatabaseSetting = contextDatabaseSetting ?? throw new Exception("Configurações do banco de dados não existem");

        ResolveServices(services);
        ResolveRepositories(services);
        ResolveMiddlewares(services);
        ResolveDatabase(services);
    }

    private static void ResolveServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPessoaService, PessoaService>();
        services.AddScoped<IStatusService, StatusService>();
        services.AddScoped<IProfissionalService, ProfissionalService>();
        services.AddScoped<IEspecialidadeService, EspecialidadeService>();
        services.AddScoped<IPerfilUsuarioService, PerfilUsuarioService>();
        services.AddScoped<IRegistroProfissionalService, RegistroProfissionalService>();
        services.AddScoped<IEnderecoService, EnderecoService>();
        services.AddScoped<IPacienteService, PacienteService>();
        services.AddScoped<IProdutoConvenioService, ProdutoConvenioService>();
        services.AddScoped<IPlanoConvenioService, PlanoConvenioService>();
        services.AddScoped<IPlanoSaudeService, PlanoSaudeService>();
        services.AddScoped<ICidService, CidService>();
        services.AddScoped<IConvenioService, ConvenioService>();
    }

    private static void ResolveRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPessoaRepository, PessoaRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();
        services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
        services.AddScoped<IEspecialidadeRepository, EspecialidadeRepository>();
        services.AddScoped<IPerfilUsuarioRepository, PerfilUsuarioRepository>();
        services.AddScoped<IRegistroProfissionalRepository, RegistroProfissionalRepository>();
        services.AddScoped<IEnderecoRepository, EnderecoRepository>();
        services.AddScoped<IPacienteRepository, PacienteRepository>();
        services.AddScoped<IProdutoConvenioRepository, ProdutoConvenioRepository>();
        services.AddScoped<IPlanoConvenioRepository, PlanoConvenioRepository>();
        services.AddScoped<IConvenioRepository, ConvenioRepository>();
    }

    private static void ResolveMiddlewares(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperConfiguration));
    }

    private static void ResolveDatabase(this IServiceCollection services)
    {
        // Inicializa os escopos que serão utilizados na execução da API
        services.Configure<ContextDatabaseSettings>(action => _contextDatabaseSetting!.Bind(action));
        services.AddSingleton<IContextDatabaseSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<ContextDatabaseSettings>>().Value);
    }
}
