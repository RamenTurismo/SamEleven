using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SamEleven.App.Steam;

internal abstract class SteamGameInfoSourceProviderBase
{
    public abstract ValueTask<IReadOnlyCollection<SteamGameInfo>> GetAllSteamGamesAsync();
}
