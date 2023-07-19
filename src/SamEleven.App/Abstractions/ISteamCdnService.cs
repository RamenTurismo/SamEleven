using System;

namespace SamEleven.App.Abstractions;

public interface ISteamCdnService
{
    Uri BuildGameImageUri(uint id, string logo);
}
