

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    public interface IDynastioCache
    {
        List<Player> Players { get; }
        List<Leaderboardcoin> Leaderboardcoin { get; }
        List<Leaderboardscore> Leaderboardscore { get; }
        List<Server> OnlineServers { get; }
        List<FeaturedVideos> FeaturedVideos { get; }
        string Changelog { get; }
    }
    public class DynastioCacheConfiguration
    {
        public int CacheTimeLeaderboardcoin { get; set; } = 3 * 60 * 1000;
        public int CacheTimeLeaderboardscore { get; set; } = 3 * 60 * 1000;
        public int CacheTimeServers { get; set; } = 30 * 1000;
        public int CacheTimeChangeLog { get; set; } = 2 * 60 * 1000;
        public int CacheTimeFeaturedVideos { get; set; } = 30 * 60 * 1000;
    }
    internal class DynastioCache : IDynastioCache, IDisposable
    {
        IDynastioProvider provider { get; }
        public readonly DynastioCacheConfiguration CacheConfiguration;
        public DynastioCache(IDynastioProvider provider, DynastioCacheConfiguration cacheConfiguration)
        {
            this.provider = provider;
            this.CacheConfiguration = cacheConfiguration;
        }
        private List<FeaturedVideos> _featuredVideosContent { get; set; } = new();
        private DateTime _featuredVideosTime = DateTime.MinValue;

        private List<Leaderboardscore> _leaderboardscoreContent { get; set; }
        private DateTime _leaderboardscoreTime = DateTime.MinValue;

        private List<Leaderboardcoin> _leaderboardcoinContent { get; set; }
        private DateTime _leaderboardcoinTime = DateTime.MinValue;

        private string _changelogContent { get; set; }
        private DateTime _changelogTime = DateTime.MinValue;

        private List<Server> _serversContent { get; set; }
        private DateTime _serversTime = DateTime.MinValue;

        private List<Player> _playersContent { get; set; }


        public List<Player> Players
        {
            get
            {
                if ((DateTime.UtcNow - _serversTime).TotalMilliseconds > CacheConfiguration.CacheTimeServers || _serversContent == null)
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
                if ((DateTime.UtcNow - _leaderboardcoinTime).TotalMilliseconds > CacheConfiguration.CacheTimeLeaderboardcoin || _leaderboardcoinContent == null)
                {

                    _leaderboardcoinContent = provider.GetCoinLeaderboardAsync().Result;
                    _leaderboardcoinTime = DateTime.UtcNow;

                }
                return _leaderboardcoinContent;
            }
        }
        public List<Leaderboardscore> Leaderboardscore
        {
            get
            {
                if ((DateTime.UtcNow - _leaderboardscoreTime).TotalMilliseconds > CacheConfiguration.CacheTimeLeaderboardscore || _leaderboardscoreContent == null)
                {
                    var list = new List<Leaderboardscore>();
                    list = provider.GetScoreLeaderboardAsync().Result;
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
                if ((DateTime.UtcNow - _serversTime).TotalMilliseconds > CacheConfiguration.CacheTimeServers || _serversContent == null)
                {
                    try
                    {
                        var servers = provider.GetOnlineServersAsync(ServerType.AllServersWithAllPlayers).Result;
                        if (servers != null)
                        {
                            _serversTime = DateTime.UtcNow;
                            _serversContent = servers;
                            _playersContent = _serversContent.SelectMany(a => a.GetPlayers()).OrderByDescending(a => a.Score).ToList();
                        }
                    }
                    catch
                    {
                        _serversContent = new();
                        _playersContent = new();
                    }
                }
                return _serversContent ?? new List<Server>();
            }
        }
        public List<FeaturedVideos> FeaturedVideos
        {
            get
            {
                if ((DateTime.UtcNow - _featuredVideosTime).TotalMilliseconds > CacheConfiguration.CacheTimeFeaturedVideos || _featuredVideosContent == null)
                {
                    try
                    {
                        _featuredVideosContent = provider.GetFeaturedVideosAsync().Result;
                        _featuredVideosTime = DateTime.UtcNow;
                    }
                    catch
                    {
                        _featuredVideosContent = new();
                    }
                }
                return _featuredVideosContent;
            }
        }
        public string Changelog
        {
            get
            {
                if ((DateTime.UtcNow - _changelogTime).TotalMilliseconds > CacheConfiguration.CacheTimeChangeLog || string.IsNullOrEmpty(_changelogContent))
                {
                    try
                    {
                        string logs = provider.GetChangeLogAsync().Result;
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
