namespace Api.Swagger;

/// <summary>
/// Configures the options for Swagger documentation generation.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ConfigureOptions"/> class.
/// </remarks>
/// <param name="provider">The API version description provider.</param>
public class ConfigureOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private SwaggerSettings _docSwagger;

    public ConfigureOptions(IApiVersionDescriptionProvider provider)
    { 
        _provider = provider;
        var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfigurationRoot configuration = builder.Build();
        var section = configuration.GetSection(SwaggerSettings.SectionName);
        var properties = section.Get<SwaggerSettings>();
     
        Contact contact = new Contact()
        { 
            Name = properties?.Contact.Name,
            Email = properties?.Contact.Email,
            Url = properties?.Contact.Url
        };
        InfoApi infoApi = new InfoApi()
        {
            Title = properties?.InfoApi.Title,
            Description = properties?.InfoApi.Description
        };
        _docSwagger = new SwaggerSettings() 
        { 
            Contact = contact,
            InfoApi = infoApi
        };

    }
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <param name="options">The Swagger generation options to configure.</param>
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, _docSwagger));
        }
    }

    /// <summary>
    /// Creates the OpenAPI information for a specific API version.
    /// </summary>
    /// <param name="description">The API version description.</param>
    /// <returns>The OpenAPI information for the API version.</returns>
    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, SwaggerSettings docSwagger)
    {
        var contact = new OpenApiContact
        {
            Name = docSwagger.Contact.Name,
            Email = docSwagger.Contact.Email,
            Url = new Uri(docSwagger.Contact.Url)
        };

        var info = new OpenApiInfo()
        {
            Title = docSwagger?.InfoApi.Title,
            Version = description.ApiVersion.ToString(),
            Description = docSwagger?.InfoApi.Description,
            Contact = contact
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}
