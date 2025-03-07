namespace Infrastructure;

/// <summary>
/// Configures the services to be added in IServiceCollection that are used in dependency injection.
/// </summary>
public static class DependencyContainer
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionSettingsSection = configuration.GetSection(ConnectionSettings.SectionName);
        var connectionSettings = connectionSettingsSection.Get<ConnectionSettings>();

        var dbHost = Environment.GetEnvironmentVariable(connectionSettings?.DBHost!);
        var dbPort = Environment.GetEnvironmentVariable(connectionSettings?.DBPort!);
        var dbInstance = Environment.GetEnvironmentVariable(connectionSettings?.DBInstance!);
        var dbName = Environment.GetEnvironmentVariable(connectionSettings?.DBName!);
        var dbUser = Environment.GetEnvironmentVariable(connectionSettings?.DBUser!);
        var dbPassword = Environment.GetEnvironmentVariable(connectionSettings?.DBPassword!);

        string connectionTemplate = connectionSettings?.DefaultConnection!;

        string connectionString = connectionTemplate
            .Replace("{SERVER}", dbHost)
            .Replace("{PORT}", dbPort)
            .Replace("{INSTANCE}", dbInstance)
            .Replace("{DB}", dbName)
            .Replace("{USER}", dbUser)
            .Replace("{PASSWORD}", dbPassword);

        services
        .AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddTransient<IRoleRepository, RoleRepository>();

        services.AddSingleton<ILoggerService, LoggerManager>();
        return services;
    }

    public static IServiceCollection AddValidatorsServices(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters(); 

        services.AddTransient<IValidator<CreateRoleDto>, CreateRoleValidator>();
        services.AddTransient<IValidator<UpdateRoleDto>, UpdateRoleValidator>();

        return services;
    }
}
