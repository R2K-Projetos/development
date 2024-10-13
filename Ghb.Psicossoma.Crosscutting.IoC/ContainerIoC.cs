using Ghb.Psicossoma.Repositories.Abstractions;
using Ghb.Psicossoma.Repositories.Context;
using Ghb.Psicossoma.Repositories.Implementations;
using Ghb.Psicossoma.Services.Abstractions;
using Ghb.Psicossoma.Services.Implementations;
using Ghb.Psicossoma.Services.MapperConfiguration;
using Ghb.Psicossoma.SharedAbstractions.Repositories.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
        services.AddScoped<ICidadeService, CidadeService>();
        services.AddScoped<ICidService, CidService>();
        services.AddScoped<IConvenioService, ConvenioService>();
        services.AddScoped<IEncaminhamentoService, EncaminhamentoService>();
        services.AddScoped<IEnderecoService, EnderecoService>();
        services.AddScoped<IEspecialidadeService, EspecialidadeService>();
        services.AddScoped<IFuncionalidadeService, FuncionalidadeService>();
        services.AddScoped<IGrauParentescoService, GrauParentescoService>();
        services.AddScoped<IGrupoGuiaService, GrupoGuiaService>();
        services.AddScoped<IPacienteService, PacienteService>();
        services.AddScoped<IPerfilUsuarioService, PerfilUsuarioService>();
        services.AddScoped<IPessoaService, PessoaService>();
        services.AddScoped<IPlanoConvenioService, PlanoConvenioService>();
        services.AddScoped<IPlanoSaudeService, PlanoSaudeService>();
        services.AddScoped<IProdutoConvenioService, ProdutoConvenioService>();
        services.AddScoped<IProfissionalService, ProfissionalService>();
        services.AddScoped<IProntuarioHistoricoService, ProntuarioHistoricoService>();
        services.AddScoped<IProntuarioService, ProntuarioService>();
        services.AddScoped<IRegistroProfissionalService, RegistroProfissionalService>();
        services.AddScoped<IStatusService, StatusService>();
        services.AddScoped<ITelefoneService, TelefoneService>();
        services.AddScoped<ITipoTelefoneService, TipoTelefoneService>();
        services.AddScoped<IUfService, UfService>();
    }

    private static void ResolveRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICidadeRepository, CidadeRepository>();
        services.AddScoped<ICidRepository, CidRepository>();
        services.AddScoped<IConvenioRepository, ConvenioRepository>();
        services.AddScoped<IEncaminhamentoRepository, EncaminhamentoRepository>();
        services.AddScoped<IEnderecoRepository, EnderecoRepository>();
        services.AddScoped<IEspecialidadeRepository, EspecialidadeRepository>();
        services.AddScoped<IFuncionalidadeRepository, FuncionalidadeRepository>();
        services.AddScoped<IGrauParentescoRepository, GrauParentescoRepository>();
        services.AddScoped<IGrupoGuiaRepository, GrupoGuiaRepository>();
        services.AddScoped<IPacienteRepository, PacienteRepository>();
        services.AddScoped<IPerfilUsuarioRepository, PerfilUsuarioRepository>();
        services.AddScoped<IPessoaRepository, PessoaRepository>();
        services.AddScoped<IPlanoConvenioRepository, PlanoConvenioRepository>();
        services.AddScoped<IPlanoSaudeRepository, PlanoSaudeRepository>();
        services.AddScoped<IProdutoConvenioRepository, ProdutoConvenioRepository>();
        services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
        services.AddScoped<IProntuarioHistoricoRepository, ProntuarioHistoricoRepository>();
        services.AddScoped<IProntuarioRepository, ProntuarioRepository>();
        services.AddScoped<IRegistroProfissionalRepository, RegistroProfissionalRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();
        services.AddScoped<ITelefoneRepository, TelefoneRepository>();        
        services.AddScoped<ITipoTelefoneRepository, TipoTelefoneRepository>();        
        services.AddScoped<IUfRepository, UfRepository>();        
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
