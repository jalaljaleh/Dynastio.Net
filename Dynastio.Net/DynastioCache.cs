

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    internal class DynastioCache : IDisposable
    {
        IDynastioProvider provider { get; }
        public DynastioCache(IDynastioProvider provider)
        {
            this.provider = provider;
        }
        private List<Leaderboardscore> _leaderboardscoreContent { get; set; }
        private DateTime _leaderboardscoreTime = DateTime.MinValue;

        private List<Leaderboardcoin> _leaderboardcoinContent { get; set; }
        private DateTime _leaderboardcoinTime = DateTime.MinValue;

        private string _changelogContent { get; set; }
        private DateTime _changelogTime = DateTime.MinValue;

        private List<Server> _serversContent { get; set; }
        private DateTime _serversTime = DateTime.MinValue;

        private List<Player> _playersContent { get; set; }

        public int CacheTimeLeaderboardcoin { get; set; } = 3 * 60 * 1000;
        public int CacheTimeLeaderboardscore { get; set; } = 3 * 60 * 1000;
        public int CacheTimeServers { get; set; } = 30 * 1000;
        public int CacheTimeChangeLog { get; set; } = 2 * 60 * 1000;

        public List<Player> Players
        {
            get
            {
                if ((DateTime.UtcNow - _serversTime).TotalMilliseconds > CacheTimeServers || _serversContent == null)
                {
                    var servers = OnlineServers;
                }
                return _playersContent;
            }
        }
        public List<Leaderboardcoin> Leaderboardcoin
        {
            get
            {
                if ((DateTime.UtcNow - _leaderboardcoinTime).TotalMilliseconds > CacheTimeLeaderboardcoin || _leaderboardcoinContent == null)
                {

                    _leaderboardcoinContent = provider.Database.GetCoinLeaderboardAsync().Result;
                    _leaderboardcoinTime = DateTime.UtcNow;

                }
                return _leaderboardcoinContent;
            }
        }
        public List<Leaderboardscore> Leaderboardscore
        {
            get
            {
                if ((DateTime.UtcNow - _leaderboardscoreTime).TotalMilliseconds > CacheTimeLeaderboardscore || _leaderboardscoreContent == null)
                {
                    var list = new List<Leaderboardscore>();
                    list = provider.Database.GetScoreLeaderboardAsync().Result;
                    _leaderboardscoreContent = list;
                    _leaderboardscoreTime = DateTime.UtcNow;
                }
                return _leaderboardscoreContent;
            }
        }

        public List<Server> OnlineServers
        {
            get
            {
                if ((DateTime.UtcNow - _serversTime).TotalMilliseconds > CacheTimeServers || _serversContent == null)
                {
                    try
                    {
                        var servers = provider.Game.GetOnlineServersAsync(ServerType.AllWithPlayers).Result;
                        if (servers != null)
                        {
                            _serversContent = servers;
                            _serversTime = DateTime.UtcNow;
                            _playersContent = _serversContent.SelectMany(a => a.GetPlayers()).OrderByDescending(a => a.Score).ToList();
                        }
                    }
                    catch { }
                }
                return _serversContent ?? new List<Server>();
            }
        }
        public string Changelog
        {
            get
            {
                if ((DateTime.UtcNow - _changelogTime).TotalMilliseconds > CacheTimeChangeLog || string.IsNullOrEmpty(_changelogContent))
                {
                    try
                    {
                        string logs = provider.Game.GetChangeLogAsync().Result;
                        _changelogContent = logs;
                        _changelogTime = DateTime.UtcNow;
                    }
                    catch { }
                }
                return _changelogContent;
            }
        }

        public void Dispose()
        {
            return;
        }
    }
}
