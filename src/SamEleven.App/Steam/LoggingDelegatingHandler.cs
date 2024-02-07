namespace SamEleven.App.Steam;

internal sealed partial class LoggingDelegatingHandler : DelegatingHandler
{
    public static partial class Log
    {
        [LoggerMessage(LogLevel.Information, "Requesting {Method} {Uri}")]
        public static partial void Sending(ILogger logger, HttpMethod method, Uri? uri);
        [LoggerMessage(LogLevel.Information, "Request {Method} {Uri} finished with {Status} in {Elapsed}ms")]
        public static partial void Received(ILogger logger, HttpMethod method, Uri? uri, HttpStatusCode status, long elapsed);
    }

    private readonly ILogger<LoggingDelegatingHandler> _logger;

    public LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Log.Sending(_logger, request.Method, request.RequestUri);

        Stopwatch requestWatch = Stopwatch.StartNew();

        HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        Log.Received(_logger, request.Method, request.RequestUri, response.StatusCode, requestWatch.ElapsedMilliseconds);

        return response;
    }
}
