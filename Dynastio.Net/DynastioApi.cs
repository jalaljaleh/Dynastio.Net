using Dynastio.Net.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    /// <summary>
    /// Primary client for interacting with Dynastio’s REST endpoints.
    /// Supports automatic caching, unified GET/POST logic, and JSON deserialization.
    /// </summary>
    public sealed class DynastioApi : IDisposable
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);
        private static readonly string BaseAuthUrl = "https://auth.dynast.cloud";
        private static readonly string BaseApiUrl = "https://dynast.cloud";

        private readonly ApiHttpClient _http;
        private readonly Random _random = new();

        // Caches for hot endpoints
        private readonly Cacheable<List<Player>> _onlinePlayersCache;
        private readonly Cacheable<List<Server>> _onlineServersCache;

        private readonly Cacheable<List<Player>> _onlineTopPlayersCache;
        private readonly Cacheable<List<Server>> _onlineServersWithPlayersCache;


        private readonly Cacheable<List<Team>> _teamsCache;

        private readonly Cacheable<Version> _versionCache;
        private readonly Cacheable<string> _changelogCache;
        private readonly Cacheable<List<LeaderboardCoin>> _coinsCache;
        private readonly Cacheable<List<LeaderboardScore>> _scoresCache;
        private readonly Cacheable<List<FeaturedVideos>> _videosCache;

        /// <summary>
        /// Construct the API client using a "key:value" token.
        /// </summary>
        public DynastioApi(string token)
        {
            var parts = token.Split(':');
            var key = parts[0];
            var secret = parts.Length == 2 ? parts[1] : throw new ArgumentException("Invalid token format");
            var versionTag = Assembly.GetEntryAssembly()?
                                       .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                                       .InformationalVersion ?? "(no version)";

            _http = new ApiHttpClient(
                key,
                secret,
                userAgent: $"dynastio.net/{versionTag}",
                timeout: DefaultTimeout
            );

            // 30s caches for dynamic lists, longer for changelog/version
            _onlinePlayersCache = new Cacheable<List<Player>>(TimeSpan.FromSeconds(30), async () => await GetPlayersAsync(true), TimeSpan.FromSeconds(15));
            _onlineTopPlayersCache = new Cacheable<List<Player>>(TimeSpan.FromSeconds(30), async () => await GetTopPlayersAsync(true), TimeSpan.FromSeconds(15));

            _onlineServersCache = new Cacheable<List<Server>>(TimeSpan.FromSeconds(30), async () => await GetServersInternalAsync(ServerType.PublicServersWithTopPlayers), TimeSpan.FromSeconds(15));
            _onlineServersWithPlayersCache = new Cacheable<List<Server>>(TimeSpan.FromSeconds(30), async () => await GetServersInternalAsync(ServerType.AllServersWithAllPlayers), TimeSpan.FromSeconds(15));

            _teamsCache = new Cacheable<List<Team>>(TimeSpan.FromSeconds(30), () => GetTeamsAsync(), TimeSpan.FromSeconds(15));
            _versionCache = new Cacheable<Version>(TimeSpan.FromMinutes(8), () => FetchJsonAsync<Version>($"{BaseApiUrl}/version.json"));
            _changelogCache = new Cacheable<string>(TimeSpan.FromMinutes(8), () => _http.GetStringAsync($"{BaseApiUrl}/changelog.txt"));
            _coinsCache = new Cacheable<List<LeaderboardCoin>>(TimeSpan.FromSeconds(250), GetLeaderboardCoinsInternalAsync);
            _scoresCache = new Cacheable<List<LeaderboardScore>>(TimeSpan.FromSeconds(250), GetLeaderboardScoresInternalAsync);
            _videosCache = new Cacheable<List<FeaturedVideos>>(TimeSpan.FromMinutes(29), GetFeaturedVideosInternalAsync);
        }

        /// <summary>All teams across all servers (cached).</summary>
        public List<Team> Teams => _teamsCache.Value;

        /// <summary>Current published version of the Dynastio API.</summary>
        public Version Version => _versionCache.Value;

        /// <summary>All players across all servers (cached).</summary>
        public List<Player> OnlinePlayers => _onlinePlayersCache.Value;
        /// <summary>All Top players across public servers (cached).</summary>
        public List<Player> OnlineTopPlayers => _onlineTopPlayersCache.Value;

        /// <summary>All servers with top players(cached).</summary>
        public List<Server> OnlineServers => _onlineServersCache.Value;
        /// <summary>All servers with full details and full players (cached).</summary>
        public List<Server> OnlineServersWithPlayers => _onlineServersWithPlayersCache.Value;

        /// <summary>Top coin leaderboard (cached).</summary>
        public List<LeaderboardCoin> LeaderboardCoins => _coinsCache.Value;

        /// <summary>Top score leaderboard (cached).</summary>
        public List<LeaderboardScore> LeaderboardScores => _scoresCache.Value;

        /// <summary>Latest changelog text (cached).</summary>
        public string Changelog => _changelogCache.Value;

        /// <summary>Featured videos (cached).</summary>
        public List<FeaturedVideos> FeaturedVideos => _videosCache.Value;

        /// <summary>
        /// Fetches all servers of the specified type.
        /// </summary>
        public async Task<List<Server>> GetServersAsync(ServerType type) => await GetServersInternalAsync(type);

        /// <summary>
        /// Fetches all players by aggregating across servers.
        /// </summary>
        public async Task<List<Player>> GetPlayersAsync(bool cache = false)
        {
            if (!cache)
            {
                var servers = await GetServersInternalAsync(ServerType.AllServersWithAllPlayers);
                _onlineServersWithPlayersCache.Update(servers);
            }
            return _onlineServersWithPlayersCache
                 .Value
                 .SelectMany(s => s.GetPlayers() ?? Array.Empty<Player>())
             .ToList();
        }
        /// <summary>
        /// Fetches top players by aggregating across servers.
        /// </summary>
        public async Task<List<Player>> GetTopPlayersAsync(bool cache = false)
        {
            if (!cache)
            {
                var servers = await GetServersInternalAsync(ServerType.AllServersWithTopPlayers);
                _onlineServersCache.Update(servers);
            }
            return _onlineServersCache
                 .Value
                 .Select(s => new Player()
                 {
                     Level = s.TopPlayerLevel,
                     Nickname = s.TopPlayerName,
                     Score = s.TopPlayerScore,
                 }.Update(s))
             .ToList();
        }
        /// <summary>
        /// Fetches all teams by aggregating across players.
        /// </summary>
        public async Task<List<Team>> GetTeamsAsync()
        {
            var servers = await GetServersInternalAsync(ServerType.AllServersWithAllPlayers);

            _onlineServersWithPlayersCache.Update(servers);

            return _onlinePlayersCache.Value
                // Group by both Server and Team name
                .GroupBy(p => new { p.Parent, TeamName = p.Team })
                .Select(group => new Team
                {
                    Id = $"{group.Key.Parent?.Label}_{group.Key.TeamName}",
                    Name = group.Key.TeamName,
                    Server = group.Key.Parent,
                    Players = group.ToList(),
                    MembersCount = group.Count()
                })
                .ToList();

        }
        /// <summary>
        /// Update a user's Discord rank on the Dynastio auth service.
        /// </summary>
        public Task<DiscordRank> UpdateDiscordRankAsync(string userId, int rank)
            => PostJsonAsync<DiscordRank, DiscordRank>($"{BaseAuthUrl}/write_api/set_user_discord_rank?uid={userId}",
                                                       new DiscordRank(rank));

        /// <summary>Checks whether a given pin code is valid for a user.</summary>
        public Task<bool> GetUserPincodeStatusAsync(string userId, string pin)
            => FetchJsonAsync<bool>($"{BaseAuthUrl}/api/check_pincode?uid={userId}&pin={pin}");

        /// <summary>Retrieves a user's overall rank.</summary>
        public async Task<UserRank> GetUserRankAsync(string userId)
        {
            var data = await FetchJsonAsync<List<int>>($"{BaseAuthUrl}/api/get_user_rank?uid={userId}");
            return new UserRank(data);
        }

        /// <summary>Retrieves a user's profile information.</summary>
        public Task<Profile> GetUserProfileAsync(string userId)
            => FetchJsonAsync<Profile>($"{BaseAuthUrl}/api/get_user_profile?uid={userId}");

        /// <summary>Retrieves a user's player stats.</summary>
        public async Task<PlayerStat> GetUserStatAsync(string userId)
        {
            var raw = await FetchJsonAsync<string>($"{BaseAuthUrl}/api/get_user_stat?uid={userId}");
            var parsed = JsonConvert.DeserializeObject(raw).ToString();
            return JsonConvert.DeserializeObject<PlayerStat>(parsed);
        }

        /// <summary>Retrieves a user's profile card (stats + chest + profile).</summary>
        public async Task<ProfileCard> GetUserProfileCardAsync(string userId)
        {
            var wrapper = await FetchJsonAsync<ProfileCardEntity>($"{BaseAuthUrl}/api/get_user_card?uid={userId}");
            // Stats
            var statRaw = JsonConvert.DeserializeObject(wrapper.Stat).ToString();
            var stat = JsonConvert.DeserializeObject<PlayerStat>(statRaw);
            // Chest
            var chest = ParseChest(wrapper.Chest);
            return new ProfileCard { Profile = wrapper.Profile, Stat = stat, Chest = chest };
        }

        /// <summary>Retrieves a user's personal chest.</summary>
        public async Task<PersonalChest> GetUserPersonalchestAsync(string userId)
        {
            var raw = await FetchJsonAsync<string>($"{BaseAuthUrl}/api/get_user_chest?uid={userId}");
            return ParseChest(raw);
        }

        /// <summary>Gets daily/weekly/monthly ranks surrounding a user.</summary>
        public async Task<UserSurroundingRank> GetUserSurroundingRankAsync(string userId)
        {
            var lists = await FetchJsonAsync<List<UserSurroundingRankRow[]>>($"{BaseAuthUrl}/leaderboard/surrounding?uid={userId}");
            return new UserSurroundingRank(userId)
            {
                UsersRankDaily = lists[0].ToList(),
                UsersRankWeekly = lists[1].ToList(),
                UsersRankMonthly = lists[2].ToList()
            };
        }

        /// <summary>Disposes the HTTP client.</summary>
        public void Dispose() => _http.Dispose();


        #region — Internal Helpers —

        private async Task<List<Server>> GetServersInternalAsync(ServerType type)
        {
            var suffix = type switch
            {
                ServerType.AllServersWithTopPlayers => "?all",
                ServerType.PublicServersWithTopPlayers => "?all",

                ServerType.AllServersWithAllPlayers => "all?full=true",
                ServerType.PublicServersWithAllPlayers => "?full=true",

                _ => ""
            };

            var url = $"https://announcement-amsterdam-0-alpaca.dynast.cloud/{suffix}&random={_random.Next()}";
            var wrapper = await _http.GetJsonAsync<DataType<List<Server>>>(url);
            return wrapper.Servers;
        }

        private async Task<List<LeaderboardCoin>> GetLeaderboardCoinsInternalAsync()
            => (await FetchJsonAsync<LeaderboardCoin[]>($"{BaseAuthUrl}/api/get_top_by_coins"))
               .ToList();

        private async Task<List<LeaderboardScore>> GetLeaderboardScoresInternalAsync()
            => await FetchJsonAsync<List<LeaderboardScore>>($"{BaseAuthUrl}/leaderboard/list_all");

        private Task<List<FeaturedVideos>> GetFeaturedVideosInternalAsync()
            => FetchJsonAsync<List<FeaturedVideos>>($"{BaseAuthUrl}/api/get_featured_videos");

        /// <summary>
        /// Performs a GET and unwraps DataType{T}.data into T.
        /// </summary>
        private async Task<T> FetchJsonAsync<T>(string url, CancellationToken ct = default)
        {
            var wrapper = await _http.GetJsonAsync<DataType<T>>(url);
            return wrapper.Data;
        }

        /// <summary>
        /// Performs a POST with JSON and unwraps DataType{TResponse}.data into TResponse.
        /// </summary>
        private async Task<TResponse> PostJsonAsync<TResponse, TRequest>(string url, TRequest body, CancellationToken ct = default)
        {
            var wrapper = await _http.PostJsonAsync<DataType<TResponse>>(url, body);
            return wrapper.Data;
        }

        /// <summary>
        /// Parses a JSON‐encoded chest string into a Personalchest instance.
        /// </summary>
        private PersonalChest ParseChest(string rawJson)
        {
            var decoded = JsonConvert.DeserializeObject(rawJson).ToString();
            var itemsArr = JObject.Parse(decoded)
                                  .SelectToken("items")?
                                  .ToObject<List<JArray>>()
                            ?? new List<JArray>();

            var items = itemsArr
                .Select(arr => new PersonalChestItem
                {
                    Index = arr[0].ToObject<int>(),
                    ItemType = (ItemType)arr[1].ToObject<int>(),
                    Count = arr[2].ToObject<int>(),
                    Durability = arr[3].ToObject<int>(),
                    Details = arr[4].ToObject<string>(),
                    OwnerId = arr[5].ToObject<string>(),
                    Token = arr[6].ToObject<string>()
                })
                .ToList();

            return new PersonalChest(items);
        }

        #endregion
    }
}
