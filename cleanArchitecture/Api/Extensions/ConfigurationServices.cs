namespace Api.Extensions;

/// <summary>
/// Provides methods for configuring Swagger.
/// </summary>
public static class ConfigurationServices
{
    /// <summary>
    /// Configures the application to use Swagger for API documentation.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <param name="builder">The WebApplicationBuilder to configure.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddSwagger(this IServiceCollection services, WebApplicationBuilder builder)
    {
        // Register the Swagger generator, defining 1 or more Swagger documents
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1.0);
            // required because the original API doesn't have a version in the URL  
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        }).AddApiExplorer(options =>
        {
            // Add the versioned API explorer
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            options.GroupNameFormat = "'v'VVV";
            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
            // can also be used to control the format of the API version in route templates
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureOptions>();
        services.AddSwaggerGen(options =>
        {
            //Following code to avoid swagger generation error 
            //due to same method name in different versions.
            options.ResolveConflictingActions(descriptions =>
            {
                return descriptions.First();
            });
            options.CustomSchemaIds(type => type.ToString());

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme (Example: '12345abcdef')",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            foreach (var doc in options.SwaggerGeneratorOptions.SwaggerDocs.Values)
            {
                doc.Title = $"{doc.Title} - Environment {builder.Environment.EnvironmentName}";
            }

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            options.OperationFilter<OperationFilter>();
            options.DocumentFilter<DocumentFilter>();
        });

        return services;
    }

    /// <summary>
    /// Configures the application to use custom Swagger UI.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <param name="app">The web application.</param>
    /// <returns>The same application builder so that multiple calls can be chained.</returns>
    public static IApplicationBuilder UseCustomSwaggerUI(this IApplicationBuilder builder, WebApplication app)
    {
        builder.UseSwaggerUI(c =>
        {
            var descriptions = app.DescribeApiVersions();

            foreach (var description in descriptions)
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                var name = $"API {description.GroupName.ToUpperInvariant()}";
                c.SwaggerEndpoint(url, name);
            }
            
            c.DefaultModelsExpandDepth(-1);
            c.InjectStylesheet("./ui/css/custom.css");
            c.DisplayRequestDuration();
            c.RoutePrefix = "swagger";
        });

        return builder;
    }

    /// <summary>
    /// Configures the application to use Serilog for logging.
    /// </summary>
    /// <param name="hostBuilder">The host builder to configure.</param>
    /// <returns>The same host builder so that multiple calls can be chained.</returns>
    public static IHostBuilder Serilog(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((hostBuilderContext, loggerConfiguration) =>
        {
            loggerConfiguration.ConfigureBaseLogging(AppDomain.CurrentDomain.FriendlyName);
        });

        return hostBuilder;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenExtractor, TokenExtractor>();

        return services;
    }

}
