using System;
using Gibbed.Steamworks;
using SamEleven.App.Abstractions;

namespace SamEleven.App.Steam.Client;

internal sealed class GibbedSteamClientWrapper : ISteamClient, IDisposable
{
    private ValueTuple<uint, GibbedSteamClient>? _activeClient;

    private GibbedSteamClient GetOrCreateClient(uint id)
    {
        if (_activeClient?.Item1 == id) return _activeClient.Value.Item2;

        GibbedSteamClient client = new();
        client.Initialize(id);

        _activeClient?.Item2.Dispose();
        _activeClient = ValueTuple.Create(id, client);

        return client;
    }

    public void Dispose()
    {
        _activeClient?.Item2.Dispose();
    }

    public SteamAppData GetAppData(uint appId)
    {
        GibbedSteamClient gibbedSteamClient = GetOrCreateClient(0);
        string? name = gibbedSteamClient.SteamApps001?.GetAppData(appId, SteamAppData.NameKey);
        string? logo = gibbedSteamClient.SteamApps001?.GetAppData(appId, SteamAppData.LogoKey);

        return new SteamAppData(name, logo);
    }
}
