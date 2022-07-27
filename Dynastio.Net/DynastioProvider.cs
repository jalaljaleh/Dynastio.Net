using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    internal class DynastioProvider : IDynastioProvider
    {
        public DynastioProviderConfiguration Configuration { get; }
        public string ProviderName { get; }
        public bool IsMainProvider { get; }
        public bool IsNightlyProvider { get => !IsMainProvider; }

        internal DynastioProvider(DynastioProviderConfiguration api, DynastioCacheConfiguration cacheConfiguration)
        {
            this.Configuration = api;
            ProviderName = Configuration.Name;
            IsMainProvider = Configuration.Name == "Main";
            ConnectionManager = new ConnectionManager(Configuration);
            ConnectionManager.Initialize();
            Cache = new DynastioCache(this, cacheConfiguration);
        }
        private readonly Random _random = new Random();
        public ConnectionManager ConnectionManager { get; }
        private IDynastioCache Cache { get; }
        public List<Server> OnlineServers { get => Cache.OnlineServers; }
        public List<Player> OnlinePlayers { get => Cache.Players; }
        public List<Leaderboardscore> Leaderboardscores { get => Cache.Leaderboardscore; }
        public List<Leaderboardcoin> Leaderboardcoins { get => Cache.Leaderboardcoin; }
        public string ChangeLog { get => Cache.Changelog; }
        public List<FeaturedVideos> FeaturedVideos { get => Cache.FeaturedVideos; }
        public async Task<List<FeaturedVideos>> GetFeaturedVideosAsync()
        {
            var result = await ConnectionManager.GetAsync<DataType<List<FeaturedVideos>>>(ConnectionManager.Api.FeaturedVideos);
            return result.Value;
        }
        public async Task<List<Server>> GetOnlineServersAsync(ServerType serverType = ServerType.AllServersWithTopPlayers)
        {
            string url = "";
            switch (serverType)
            {
                default:
                case ServerType.PublicServersWithTopPlayers:
                    url = ConnectionManager.Api.Servers;
                    break;
                case ServerType.AllServersWithTopPlayers:
                    url = ConnectionManager.Api.AllServers;
                    break;
                case ServerType.AllServersWithAllPlayers:
                    url = ConnectionManager.Api.AllServersWithPlayers;
                    break;
                case ServerType.PublicServersWithAllPlayers:
                    url = ConnectionManager.Api.ServersWithPlayers;
                    break;
            }

            var result = await ConnectionManager.GetAsync<DataType<List<Server>>>(ConnectionManager.Api.ServersBaseAddress + url);
            return result.Servers;
        }
        public async Task<List<Player>> GetOnlinePlayersAsync(ServerType serverType = ServerType.PublicServersWithTopPlayers)
        {
            var servers = await GetOnlineServersAsync(serverType);
            return servers.SelectMany(c => c.Players ?? new()).ToList() ?? new();
        }
        public async Task<Version> GetCurrentVersionAsync()
        {
            return await ConnectionManager.GetAsync<Version>(ConnectionManager.Api.Version + "?random=" + _random.Next());
        }
        public async Task<string> GetChangeLogAsync()
        {
            return await ConnectionManager.GetAsync(ConnectionManager.Api.Changelog + "?random=" + _random.Next());
        }
        public async Task<List<Leaderboardcoin>> GetCoinLeaderboardAsync()
        {
            var result = await ConnectionManager.GetAsync<DataType<Leaderboardcoin[]>>(ConnectionManager.Api.LeaderboardCoin);
            return result.Value.ToList();
        }
        public async Task<List<Leaderboardscore>> GetScoreLeaderboardAsync(LeaderboardType leaderboardType)
        {
            var result = await ConnectionManager.GetAsync<DataType<List<Leaderboardscore[]>>>(ConnectionManager.Api.LeaderboardScore);
            return result.Value.ToArray()[(int)leaderboardType].ToList();
        }
        public async Task<List<Leaderboardscore>> GetScoreLeaderboardAsync()
        {
            var result = await ConnectionManager.GetAsync<DataType<List<Leaderboardscore[]>>>(ConnectionManager.Api.LeaderboardScore);
            return result.Value.SelectMany(a => a).ToList();
        }

        public async Task<UserRank> GetUserRanAsync(string playerId)
        {
            var result = await ConnectionManager.GetAsync<DataType<List<int>>>(ConnectionManager.Api.UserRank + playerId);
            return new UserRank(result.data);
        }
        public async Task<PlayerStat> GetUserStatAsync(string playerId)
        {
            var result = await ConnectionManager.GetAsync<DataType<string>>(ConnectionManager.Api.UserStat + playerId);
            return result.DeserializeObjectData<string, PlayerStat>();
        }
        public async Task<Profile> GetUserProfileAsync(string playerId)
        {
            var data = await ConnectionManager.GetAsync<DataType<Profile>>(ConnectionManager.Api.UserProfile + playerId);

            var details = await GetUserProfileDetailsAsync(playerId);
            data.Value.Details = details;
            return data.Value;
        }
        public async Task<ProfileDetails> GetUserProfileDetailsAsync(string playerId)
        {
            var result = await ConnectionManager.GetAsync<DataType<string>>(ConnectionManager.Api.UserProfileDetails + playerId);
            var data = result.DeserializeObjectData<string, JObject>(); ;
            var values = JObject.Parse(data.ToString());
            return new ProfileDetails()
            {
                Experience = int.Parse(values["experience"].ToString()),
                Level = int.Parse(values["level"].ToString()) + 1
            };
        }
        public async Task<Personalchest> GetUserPersonalchestAsync(string PlayerId)
        {
            return new Personalchest()
            {
                Items = await GetUserPersonalchestItemsAsync(PlayerId)
            };
        }
        public async Task<List<PersonalChestItem>> GetUserPersonalchestItemsAsync(string playerId)
        {
            var result = await ConnectionManager.GetAsync<DataType<string>>(ConnectionManager.Api.UserChest + playerId);
            var data = result.DeserializeObjectData<string, JObject>().SelectToken("items").ToArray();
            var chestItems = new List<PersonalChestItem>();
            foreach (var item in data)
            {
                var item_ = new PersonalChestItem()
                {
                    index = int.Parse(item[0].ToString()),
                    ItemType = (ItemType)int.Parse(item[1].ToString()),
                    Count = int.Parse(item[2].ToString()),
                    Durablity = int.Parse(item[3].ToString()),
                    Details = item[4].ToString(),
                    OwnerID = item[5].ToString(),
                    Token = item[6].ToString()
                };
                chestItems.Add(item_);
            }
            return chestItems;
        }
        public async Task<bool> IsUserAccountExistAsync(string Id)
        {
            try { var data = await GetUserProfileDetailsAsync(Id); return true; } catch { return false; };
        }
        public async Task<UserSurroundingRank> GetUserSurroundingRankAsync(string playerId)
        {
            var result = await ConnectionManager.GetAsync<DataType<List<UserSurroundingRankRow[]>>>(ConnectionManager.Api.UserSurroundingRank + playerId);
            return new UserSurroundingRank(playerId)
            {
                UsersRankDaily = result.data[0].ToList(),
                UsersRankWeekly = result.data[1].ToList(),
                UsersRankMontly = result.data[2].ToList()
            };
        }


        public void Dispose()
        {
            ConnectionManager.Dispose();
        }
    }
}
