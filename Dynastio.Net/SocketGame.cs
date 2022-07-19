using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    internal class SocketGame : ISocketGame
    {
        public SocketGame(DynastioProvider provider, DynastioProviderConfiguration connectionManager)
        {
            ConnectionManager = new ConnectionManager(connectionManager);
            ConnectionManager.Initialize();
            Cache = new DynastioCache(provider);
        }
        private readonly Random _random = new Random();
        internal ConnectionManager ConnectionManager { get; set; }
        public DynastioCache Cache { get; set; }

        public List<Server> OnlineServers { get => Cache.OnlineServers; }
        public List<Player> OnlinePlayers { get => Cache.Players; }
        public string ChangeLog { get => Cache.Changelog; }

        public async Task<List<Server>> GetOnlineServers(ServerType serverType = ServerType.Public)
        {
            string url = "";
            switch (serverType)
            {
                default:
                case ServerType.Public:
                    url = ConnectionManager.Api.Servers;
                    break;
                case ServerType.All:
                    url = ConnectionManager.Api.AllServers;
                    break;
                case ServerType.AllWithPlayers:
                    url = ConnectionManager.Api.AllServersWithPlayers;
                    break;
                case ServerType.PublicWithPlayers:
                    url = ConnectionManager.Api.ServersWithPlayers;
                    break;
            }

            var result = await ConnectionManager.GetAsync<DataType<List<Server>>>(ConnectionManager.Api.ServersBaseAddress + url + "&random=" + _random.Next());
            return result.Servers;
        }
        public async Task<List<Player>> GetOnlinePlayers(ServerType serverType = ServerType.All, bool OnlyAuthUsers = false)
        {
            var servers = await GetOnlineServers(serverType);
            return OnlyAuthUsers ? servers.SelectMany(a => a.Players.Where(b => !string.IsNullOrEmpty(b.Id))).ToList() : servers.SelectMany(c => c.Players).ToList();
        }
        public async Task<Version> GetCurrentVersionAsync()
        {
            return await ConnectionManager.GetAsync<Version>(ConnectionManager.Api.Version + "?random=" + _random.Next());
        }
        public async Task<string> GetChangeLogAsync()
        {
            return await ConnectionManager.GetAsync(ConnectionManager.Api.Changelog + "?random=" + _random.Next());
        }

        public void Dispose()
        {
            ConnectionManager.Dispose();
        }
    }


}
