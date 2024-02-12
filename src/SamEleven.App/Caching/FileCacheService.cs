namespace SamEleven.App.Caching;

internal sealed class FileCacheService : IFileCacheService
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

    public async ValueTask<T?> GetAsync<T>(string path, CancellationToken cancellationToken = default)
    {
        string fullPath = GetPath(path);

        if (!File.Exists(fullPath)) return default;

        using FileStream reader = File.OpenRead(fullPath);
        return await JsonSerializer.DeserializeAsync<T>(reader, _serializerOptions, cancellationToken).ConfigureAwait(false);
    }

    public async Task SaveAsync<T>(string path, T value, CancellationToken cancellationToken = default)
    {
        using FileStream reader = File.OpenWrite(GetPath(path));
        await JsonSerializer.SerializeAsync(reader, value, _serializerOptions, cancellationToken).ConfigureAwait(false);
    }

    private string GetPath(string path) => Path.Combine(_options.RootPath, path);
}
