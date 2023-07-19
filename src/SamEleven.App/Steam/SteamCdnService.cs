using System;
using Microsoft.Extensions.Options;
using SamEleven.App.Abstractions;

namespace SamEleven.App.Steam;

internal sealed class SteamCdnService : ISteamCdnService
{
    private readonly SteamCdnOptions _options;

    public SteamCdnService(IOptions<SteamCdnOptions> options)
    {
        _options = options.Value;
    }

    public Uri BuildGameImageUri(uint id, string logo)
    {
        return new Uri(string.Format(_options.GameImageUrl, id, logo));
    }
}
