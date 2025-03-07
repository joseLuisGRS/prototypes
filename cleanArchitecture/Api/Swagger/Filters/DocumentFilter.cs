namespace Api.Swagger.Filters;

/// <summary>
/// A filter to modify the Swagger documentation.
/// </summary>
public class DocumentFilter : IDocumentFilter
{
    /// <summary>
    /// Applies the filter to the Swagger documentation.
    /// </summary>
    /// <param name="swaggerDoc">The Swagger document to apply the filter to.</param>
    /// <param name="context">The context of the document filter.</param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (swaggerDoc == null)
        {
            throw new ArgumentNullException(nameof(swaggerDoc));
        }

        var replacements = new OpenApiPaths();

        foreach (var (key, value) in swaggerDoc.Paths)
        {
            replacements.Add(key.Replace("{version}", swaggerDoc.Info.Version,
                    StringComparison.InvariantCulture), value);
        }

        swaggerDoc.Paths = replacements;
    }
}
