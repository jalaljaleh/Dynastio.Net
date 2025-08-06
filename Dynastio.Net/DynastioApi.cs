
using Dynastio.Net.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    public class DynastioApi : IDisposable
    {

        private const string BaseAuthUrl = "https://auth.dynast.cloud";
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(20);
        private readonly ApiHttpClient _http;
        private readonly Random _random = new Random();

        private readonly Cacheable<List<Player>> _players;
        private readonly Cacheable<List<Server>> _servers;
        private readonly Cacheable<Version> _versionCache;
        private readonly Cacheable<string> _changelog;
        private readonly Cacheable<List<Leaderboardcoin>> _leaderboardcoin;
        private readonly Cacheable<List<Leaderboardscore>> _leaderboardscore;
        private readonly Cacheable<List<FeaturedVideos>> _featuredVideos;

        public DynastioApi(string token)
        {
            var parts = token.Split(':');
            var tokenKey = parts[0];
            var tokenValue = parts[1];

            _http = new ApiHttpClient(
                tokenKey,
                tokenValue,
                userAgent: $"dynastio.net/{GetVersionFromAssembly()}",
                timeout: _timeout
            );
            string GetVersionFromAssembly()
            {
                var version = Assembly.GetExecutingAssembly()
                                      .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                      ?.InformationalVersion;

                return version ?? "(No version found)";
            }

            _players = new Cacheable<List<Player>>(TimeSpan.FromSeconds(30), GetPlayersAsync);
            _servers = new Cacheable<List<Server>>(TimeSpan.FromSeconds(30), () => GetServersAsync(ServerType.AllServersWithAllPlayers));
            _versionCache = new Cacheable<Version>(TimeSpan.FromSeconds(500), GetVersionAsync);
            _changelog = new Cacheable<string>(TimeSpan.FromSeconds(500), GetChangeLogAsync);
            _leaderboardcoin = new Cacheable<List<Leaderboardcoin>>(TimeSpan.FromSeconds(250), GetLeaderboardcoinsAsync);
            _leaderboardscore = new Cacheable<List<Leaderboardscore>>(TimeSpan.FromSeconds(250), GetLeaderboardscoresAsync);
            _featuredVideos = new Cacheable<List<FeaturedVideos>>(TimeSpan.FromMinutes(29), GetFeaturedVideosAsync);
        }

        public Version Version => _versionCache.Value;
        public List<Player> OnlinePlayers => _players.Value;
        public List<Server> OnlineServers => _servers.Value;
        public List<Leaderboardcoin> Leaderboardcoins => _leaderboardcoin.Value;
        public List<Leaderboardscore> Leaderboardscore => _leaderboardscore.Value;
        public string Changelog => _changelog.Value;
        public List<FeaturedVideos> FeaturedVideos => _featuredVideos.Value;

        public async Task<List<Server>> GetServersAsync(ServerType serverType)
        {
            string urlPart = serverType switch
            {
                ServerType.AllServersWithAllPlayers => "all?full=true",
                ServerType.PublicServersWithAllPlayers => "?full=true",
                ServerType.AllServersWithTopPlayers => "all",
                _ => ""
            };

            var fullUrl = $"https://announcement-amsterdam-0-alpaca.dynast.cloud/{urlPart}&random={_random.Next()}";
            var raw = await _http.GetStringAsync(fullUrl);
            var data = JsonConvert.DeserializeObject<DataType<List<Server>>>(raw);
            return data.Servers;
        }

        public async Task<List<Player>> GetPlayersAsync()
            => (await GetServersAsync(ServerType.AllServersWithAllPlayers))
               .SelectMany(s => s.GetPlayers() ?? new List<Player>())
               .ToList();

        public async Task<Version> GetVersionAsync()
            => await _http.GetJsonAsync<Version>("https://dynast.cloud/version.json");

        public async Task<string> GetChangeLogAsync()
            => await _http.GetStringAsync("https://dynast.cloud/changelog.txt");

        public async Task<DiscordRank> UpdateDiscordRank(string accountId, int rank)
        {
            var result = await _http.PostJsonAsync<DataType<DiscordRank>>(
                $"{BaseAuthUrl}/write_api/set_user_discord_rank?uid={accountId}",
                new DiscordRank(rank)
            );
            return result.data;
        }

        public async Task<List<Leaderboardcoin>> GetLeaderboardcoinsAsync()
        {
            var result = await _http.GetJsonAsync<DataType<Leaderboardcoin[]>>(
                $"{BaseAuthUrl}/api/get_top_by_coins");
            return result.data.ToList();
        }

        public async Task<bool> GetUserPincodeStatusAsync(string Id, string pincode)
        {
            var result = await _http.GetJsonAsync<DataType<bool>>(
                $"{BaseAuthUrl}/api/check_pincode?uid={Id}&pin={pincode}");
            return result.data;
        }

        public async Task<List<Leaderboardscore>> GetLeaderboardscoresAsync()
        {
            var result = await _http.GetJsonAsync<DataType<List<Leaderboardscore>>>(
                $"{BaseAuthUrl}/leaderboard/list_all");
            return result.data;
        }

        public async Task<UserRank> GetUserRankAsync(string playerId)
        {
            var result = await _http.GetJsonAsync<DataType<List<int>>>(
                $"{BaseAuthUrl}/api/get_user_rank?uid={playerId}");
            return new UserRank(result.data);
        }

        public async Task<PlayerStat> GetUserStatAsync(string playerId)
        {
            var result = await _http.GetJsonAsync<DataType<string>>(
                $"{BaseAuthUrl}/api/get_user_stat?uid={playerId}");
            var clear = JsonConvert.DeserializeObject(result.data).ToString();
            return JsonConvert.DeserializeObject<PlayerStat>(clear);
        }

        public async Task<ProfileCard> GetUserProfileCardAsync(string playerId)
        {
            var result = await _http.GetJsonAsync<DataType<ProfileCardEntitiy>>(
                $"{BaseAuthUrl}/api/get_user_card?uid={playerId}");

            var statClear = JsonConvert.DeserializeObject(result.data.Stat).ToString();
            var stat = JsonConvert.DeserializeObject<PlayerStat>(statClear);

            var chestClear = JsonConvert.DeserializeObject(result.data.Chest).ToString();
            var chest = ParseToChest(chestClear);

            return new ProfileCard
            {
                Profile = result.data.Profile,
                Chest = chest,
                Stat = stat
            };
        }

        public async Task<Profile> GetUserProfileAsync(string playerId)
        {
            var result = await _http.GetJsonAsync<DataType<Profile>>(
                $"{BaseAuthUrl}/api/get_user_profile?uid={playerId}");
            return result.data;
        }

        public async Task<Personalchest> GetUserPersonalchestAsync(string playerId)
        {
            var result = await _http.GetJsonAsync<DataType<string>>(
                $"{BaseAuthUrl}/api/get_user_chest?uid={playerId}");
            return ParseToChest(result.data);
        }

        private Personalchest ParseToChest(string data)
        {
            var clear = JsonConvert.DeserializeObject(data).ToString();
            var itemsArr = JsonConvert.DeserializeObject<JObject>(clear)
                                        .SelectToken("items")
                                        .ToArray();

            var items = itemsArr.Select(item => new PersonalChestItem
            {
                index = (int)item[0],
                ItemType = (ItemType)(int)item[1],
                Count = (int)item[2],
                Durablity = (int)item[3],
                Details = (string)item[4],
                OwnerID = (string)item[5],
                Token = (string)item[6]
            }).ToList();

            return new Personalchest(items);
        }

        public async Task<UserSurroundingRank> GetUserSurroundingRankAsync(string playerId)
        {
            var result = await _http.GetJsonAsync<DataType<List<UserSurroundingRankRow[]>>>(
                $"{BaseAuthUrl}/leaderboard/surrounding?uid={playerId}");

            return new UserSurroundingRank(playerId)
            {
                UsersRankDaily = result.data[0].ToList(),
                UsersRankWeekly = result.data[1].ToList(),
                UsersRankMontly = result.data[2].ToList()
            };
        }

        public async Task<List<FeaturedVideos>> GetFeaturedVideosAsync()
        {
            var result = await _http.GetJsonAsync<DataType<List<FeaturedVideos>>>(
                $"{BaseAuthUrl}/api/get_featured_videos");
            return result.data;
        }

        public void Dispose() => _http.Dispose();
    }
}
