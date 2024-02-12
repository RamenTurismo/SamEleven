namespace SamEleven.App.Abstractions;

public interface IDispatcherQueueService
{
    Task Enqueue(Action func, CancellationToken cancellationToken = default);
}
