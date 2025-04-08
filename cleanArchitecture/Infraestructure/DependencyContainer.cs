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

        var jwtSettingsSection = configuration.GetSection(JwtSettings.SectionName);
        var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
        var jwtSecrets = Environment.GetEnvironmentVariable(jwtSettings?.Secrets!);

        services
        .Configure<JwtSettings>(jwtSettingsSection)
        .AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        })
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidAudience = jwtSettings.Audience,
                ValidIssuer = jwtSettings.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecrets)
                ),
            };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = async context =>
                {
                    if (context.Response.Headers.ContainsKey("X-Error-Handled"))
                    {
                        context.HandleResponse();
                        return;
                    }

                    context.HandleResponse();

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var errorResponse = ApiResponse<string>.ErrorResponse("Invalid access.");

                    var result = JsonSerializer.Serialize(errorResponse);

                    await context.Response.WriteAsync(result);

                },
                OnAuthenticationFailed = async context =>
                {
                    context.Response.Headers.Add("X-Error-Handled", "true");

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var errorResponse = ApiResponse<string>.ErrorResponse("Invalid access.");

                    var result = JsonSerializer.Serialize(errorResponse);

                    await context.Response.WriteAsync(result);
                },
                OnForbidden = async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";

                    var errorResponse = ApiResponse<string>.ErrorResponse("Invalid access.");

                    var result = JsonSerializer.Serialize(errorResponse);

                    await context.Response.WriteAsync(result);
                }
            };
        });

        services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddTransient<IRoleRepository, RoleRepository>();
        services.AddTransient<IUserRepository, UserRepository>();

        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<ILoggerService, LoggerManager>();
        return services;
    }

    public static IServiceCollection AddValidatorsServices(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        services.AddTransient<IValidator<CreateRoleDto>, CreateRoleValidator>();
        services.AddTransient<IValidator<UpdateRoleDto>, UpdateRoleValidator>();
        services.AddTransient<IValidator<RefreshTokenDto>, RefreshTokenValidator>();
        services.AddTransient<IValidator<AuthenticateDto>, AuthenticateValidator>();

        return services;
    }

}
