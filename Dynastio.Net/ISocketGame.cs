using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    public interface ISocketGame : IDisposable
    {
        Task<List<Player>> GetOnlinePlayers(ServerType serverType = ServerType.All, bool OnlyAuthUsers = false);
        Task<List<Server>> GetOnlineServers(ServerType serverType = ServerType.Public);
        Task<Version> GetCurrentVersionAsync();
        Task<string> GetChangeLogAsync();
        List<Server> OnlineServers { get; }
        List<Player> OnlinePlayers { get; }
        string ChangeLog { get; }
    }
}
