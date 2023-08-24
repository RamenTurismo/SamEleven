using SamEleven.App.Steam;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SamEleven.App.Abstractions;

public interface ISteamService
{
    ValueTask<IReadOnlyList<SteamGameInfo>> GetAllGamesAsync();
}
