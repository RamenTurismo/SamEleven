namespace SamEleven.App.Caching;

internal sealed class FileCacheService
{
    private readonly FileCacheServiceOptions _options;
    private readonly JsonSerializerOptions _serializerOptions;

    public FileCacheService(IOptions<FileCacheServiceOptions> options)
    {
        _options = options.Value;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            TypeInfoResolver = FileCacheJsonContext.Default
        };
    }

    public ValueTask<T?> GetAsync<T>(string path, CancellationToken cancellationToken = default)
    {
        using StreamReader reader = new(GetPath(path), Encoding.UTF8);
        return JsonSerializer.DeserializeAsync<T>(reader.BaseStream, _serializerOptions, cancellationToken);
    }

    public Task SaveAsync<T>(string path, T value, CancellationToken cancellationToken = default)
    {
        using StreamWriter reader = new(GetPath(path), append: false, Encoding.UTF8);
        return JsonSerializer.SerializeAsync(reader.BaseStream, value, _serializerOptions, cancellationToken);
    }

    private string GetPath(string path) => Path.Combine(_options.RootPath, path);
}
