namespace Emyfreya.Steam.Web.Api.Models;

public sealed record GetAppListResult
{
    [JsonPropertyName("applist")]
    public required GetAppListAppListResult AppList { get; init; }
}

public sealed record GetAppListAppListResult(GetAppListAppResult[] Apps);

public sealed record GetAppListAppResult
{
    [JsonPropertyName("appid")]
    public required uint AppId { get; init; }
    public required string Name { get; init; }
}
