using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    public class DynastioClient : IDisposable
    {
        public DynastioClient()
        {
            AddDefaultProviders("token:unauthenticated");
        }
        public DynastioClient(string token)
        {
            AddDefaultProviders(token);
        }

        public List<IDynastioProvider> Providers { get; set; } = new List<IDynastioProvider>();
        public IDynastioProvider Main { get => Providers.FirstOrDefault(a => a.IsMainProvider); }
        public IDynastioProvider Nightly { get => Providers.FirstOrDefault(a => a.ProviderName.ToLower() == "nightly"); }
        public IDynastioProvider GetProvider(string name) => Providers.FirstOrDefault(a => a.ProviderName.ToLower() == name.ToLower());
        public IDynastioProvider this[string name] { get => GetProvider(name); }

        DynastioClient AddProvider(params DynastioProviderConfiguration[] configurations)
        {
            foreach (var d in configurations) Providers.Add(new DynastioProvider(d));
            return this;
        }
        private void AddDefaultProviders(string token)
        {
            if (!token.Contains(":")) throw new Exception("Invalid Dynast.io Token Format.");

            AddProvider(new DynastioProviderConfiguration()
            {
                Name = "Main",
                BaseAddress = "https://auth.dynast.io/",
                AuthorizationKey = token.Split(':')[0],
                AuthorizationValue = token.Split(':')[1],
                LeaderboardCoin = "api/get_top_by_coins",
                LeaderboardScore = "leaderboard/list_all",
                UserProfile = "api/get_user_profile?uid=",
                UserChest = "api/get_user_chest?uid=",
                UserStat = "api/get_user_stat?uid=",
                UserProfileDetails = "api/get_user_rank?uid=",
                UserRank = "leaderboard/position?uid=",
                UserSurroundingRank = "leaderboard/surrounding?uid=",
                ServersBaseAddress = "https://announcement-amsterdam-0-alpaca.dynast.cloud/",
                Servers = "#",
                AllServers = "all",
                ServersWithPlayers = "?full=true",
                AllServersWithPlayers = "all?full=true",
                FeaturedVideos= "api/get_featured_videos",
                Version = "https://dynast.io/version.json",
                Changelog = "https://dynast.io/changelog.txt"
            },
            new DynastioProviderConfiguration()
            {
                Name = "Nightly",
                BaseAddress = "https://auth.nightly.dynast.io/",
                AuthorizationKey = token.Split(':')[0],
                AuthorizationValue = token.Split(':')[1],
                LeaderboardCoin = "api/get_top_by_coins",
                LeaderboardScore = "leaderboard/list_all",
                UserProfile = "api/get_user_profile?uid=",
                UserChest = "api/get_user_chest?uid=",
                UserStat = "api/get_user_stat?uid=",
                UserProfileDetails = "api/get_user_rank?uid=",
                UserRank = "leaderboard/position?uid=",
                UserSurroundingRank = "leaderboard/surrounding?uid=",
                ServersBaseAddress = "https://nightly-announcement-alpaca.dynast.cloud/",
                Servers = "#",
                AllServers = "all",
                ServersWithPlayers = "?full=true",
                AllServersWithPlayers = "all?full=true",
                FeaturedVideos = "api/get_featured_videos",
                Version = "https://nightly.dynast.io/version.json",
                Changelog = "https://nightly.dynast.io/changelog.txt"
            });

        }
        public void Dispose()
        {
            foreach (var p in Providers) p.Dispose();
        }
    }
}
