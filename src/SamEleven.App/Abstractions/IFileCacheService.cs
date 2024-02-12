namespace SamEleven.App.Abstractions;

public interface IFileCacheService
{
    ValueTask<T?> GetAsync<T>(string path, CancellationToken cancellationToken = default);
    Task SaveAsync<T>(string path, T value, CancellationToken cancellationToken = default);
}
