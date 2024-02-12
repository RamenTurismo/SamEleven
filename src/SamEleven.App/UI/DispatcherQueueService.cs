namespace SamEleven.App.UI;

internal sealed class DispatcherQueueService : IDispatcherQueueService
{
    private readonly TaskScheduler _scheduler;

    public DispatcherQueueService()
    {
        _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
    }

    public Task Enqueue(Action func, CancellationToken cancellationToken = default)
    {
        return Task.Factory.StartNew(func, cancellationToken, TaskCreationOptions.None, _scheduler);
    }
}
