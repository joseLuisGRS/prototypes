namespace Application.Mapping;

/// <summary>
/// Implementation of IMapper to be used with dependency injection.
/// </summary>
public class MapperService : IMapper
{
    private readonly TypeAdapterConfig _config;

    /// <summary>
    /// Creates a new instance of ServiceMapper with the provided configuration.
    /// </summary>
    public MapperService()
    {
        _config = TypeAdapterConfig.GlobalSettings;
    }

    /// <summary>
    /// Adapts the source object to the destination type.
    /// </summary>
    /// <typeparam name="TDestination">Destination type</typeparam>
    /// <param name="source">Source object</param>
    /// <returns>Mapped destination object</returns>
    public TDestination Map<TDestination>(object source)
    {
        return source.Adapt<TDestination>(_config);
    }

    /// <summary>
    /// Adapts the source object to the destination type.
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestination">Destination type</typeparam>
    /// <param name="source">Source object</param>
    /// <returns>Mapped destination object</returns>
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return source.Adapt<TSource, TDestination>(_config);
    }

    /// <summary>
    /// Adapts the source object to the existing destination object.
    /// </summary>
    /// <typeparam name="TDestination">Destination type</typeparam>
    /// <param name="source">Source object</param>
    /// <param name="destination">Destination object</param>
    /// <returns>The destination object</returns>
    public TDestination Map<TDestination>(object source, TDestination destination)
    {
        return source.Adapt(destination, _config);
    }

    /// <summary>
    /// Adapts the source object to the existing destination object.
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestination">Destination type</typeparam>
    /// <param name="source">Source object</param>
    /// <param name="destination">Destination object</param>
    /// <returns>The destination object</returns>
    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        return source.Adapt(destination, _config);
    }

    /// <summary>
    /// Configure the new adapt.
    /// </summary>
    /// <typeparam name="TSource">Source object</typeparam>
    /// <param name="source">Source object</param>
    /// <returns>The destination object</returns>
    public ITypeAdapterBuilder<TSource> From<TSource>(TSource source)
    {
        return (ITypeAdapterBuilder<TSource>)_config.NewConfig<TSource, object>();
    }

    /// <summary>
    /// Adapts the source object to the existing destination object with the specified destination type.
    /// </summary>
    /// <param name="source">Source object</param>
    /// <param name="sourceType">SourceType object</param>
    /// <param name="destinationType">The destination type</param>
    /// <returns></returns>
    public object Map(object source, Type sourceType, Type destinationType)
    {
        return source.Adapt(sourceType, destinationType);
    }

    /// <summary>
    /// Adapts the source object to the existing destination object with the specified destination type.
    /// </summary>
    /// <param name="source">Source object</param>
    /// <param name="destination">The destination object</param>
    /// <param name="sourceType">Source type</param>
    /// <param name="destinationType">The destination type</param>
    /// <returns></returns>
    public object Map(object source, object destination, Type sourceType, Type destinationType)
    {
        return source.Adapt(sourceType, destinationType, (TypeAdapterConfig)destination);
    }

    /// <summary>
    /// Gets the TypeAdapterConfig used by this mapper.
    /// </summary>
    public TypeAdapterConfig Config => _config;

}