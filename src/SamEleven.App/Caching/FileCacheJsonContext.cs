namespace SamEleven.App.Caching;

[JsonSerializable(typeof(List<SteamApp>))]
internal partial class FileCacheJsonContext : JsonSerializerContext;
