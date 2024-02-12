namespace SamEleven.App.Caching;

[JsonSerializable(typeof(SteamGameInfo[]))]
internal partial class FileCacheJsonContext : JsonSerializerContext;
