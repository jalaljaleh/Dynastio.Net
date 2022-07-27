using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    public interface ISocketGame : IDisposable
    {
        Task<List<Player>> GetOnlinePlayersAsync(ServerType serverType = ServerType.All);
        Task<List<Server>> GetOnlineServersAsync(ServerType serverType = ServerType.Public);
        Task<Version> GetCurrentVersionAsync();
        Task<string> GetChangeLogAsync();
        Task<List<FeaturedVideos>> GetFeaturedVideosAsync();
        List<Server> OnlineServers { get; }
        List<Player> OnlinePlayers { get; }
        string ChangeLog { get; }
    }
}
