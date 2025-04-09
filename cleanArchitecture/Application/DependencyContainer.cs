namespace Application;

/// <summary>
/// Configures the services to be added in IServiceCollection that are used in dependency injection.
/// </summary>
public static class DependencyContainer
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IAuthService, AuthService>();

        var config = new TypeAdapterConfig();
        AutoMapperProfiles.RegisterMappings();
        services.AddSingleton(config);
        services.AddSingleton<IMapper, MapperService>();

        return services;
    }
}
