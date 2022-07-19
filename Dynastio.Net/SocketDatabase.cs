using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    internal class SocketDatabase : ISocketDatabase
    {
        public SocketDatabase(DynastioProvider provider, DynastioProviderConfiguration configuration)
        {
            {
                ConnectionManager = new ConnectionManager(configuration);
                ConnectionManager.Initialize();
                Cache = new DynastioCache(provider);
            }
        }
        internal ConnectionManager ConnectionManager { get; set; }
        public DynastioCache Cache { get; set; }

        public List<Leaderboardscore> Leaderboardscores { get => Cache.Leaderboardscore; }
        public List<Leaderboardcoin> Leaderboardcoins { get => Cache.Leaderboardcoin; }
        public async Task<List<Leaderboardcoin>> GetCoinLeaderboardAsync()
        {
            try
            {
                var result = await ConnectionManager.GetAsync<DataType<Leaderboardcoin[]>>(ConnectionManager.Api.LeaderboardCoin);
                return result.Value.ToList();
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<Leaderboardscore>> GetScoreLeaderboardAsync(LeaderboardType leaderboardType)
        {
            try
            {
                var result = await ConnectionManager.GetAsync<DataType<List<Leaderboardscore[]>>>(ConnectionManager.Api.LeaderboardScore);
                return result.Value.ToArray()[(int)leaderboardType].ToList();
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<Leaderboardscore>> GetScoreLeaderboardAsync()
        {
            try
            {
                var result = await ConnectionManager.GetAsync<DataType<List<Leaderboardscore[]>>>(ConnectionManager.Api.LeaderboardScore);
                return result.Value.SelectMany(a => a).ToList();
            }
            catch
            {
                return null;
            }
        }

        public async Task<UserRank> GetUserRank(string playerId)
        {
            try
            {
                var result = await ConnectionManager.GetAsync<DataType<List<int>>>(ConnectionManager.Api.UserRank + playerId);
                return new UserRank(result.data);
            }
            catch
            {
                return null;
            }
        }
        public async Task<PlayerStat> GetUserStatAsync(string playerId)
        {
            try
            {
                var result = await ConnectionManager.GetAsync<DataType<string>>(ConnectionManager.Api.UserStat + playerId);
                return result.DeserializeObjectData<string, PlayerStat>();
            }
            catch
            {
                return null;
            }
        }
        public async Task<Profile> GetUserProfileAsync(string playerId)
        {
            try
            {
                var data = await ConnectionManager.GetAsync<DataType<Profile>>(ConnectionManager.Api.UserProfile + playerId);

                var details = await GetUserProfileDetailsAsync(playerId);
                data.Value.Details = details;
                return data.Value;
            }
            catch
            {
                return null;
            }
        }
        public async Task<ProfileDetails> GetUserProfileDetailsAsync(string playerId)
        {
            try
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
            catch
            {
                return null;
            }
        }
        public async Task<Personalchest> GetUserPersonalchestAsync(string PlayerId)
        {
            try
            {
                return new Personalchest()
                {
                    Items = await GetUserPersonalchestItemsAsync(PlayerId)
                };
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<PersonalChestItem>> GetUserPersonalchestItemsAsync(string playerId)
        {
            try
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
            catch
            {
                return new List<PersonalChestItem>();
            }
        }
        public async Task<bool> IsUserAccountExistAsync(string Id)
        {
            try { var data = await GetUserProfileDetailsAsync(Id); return true; } catch { return false; };
        }
        public async Task<UserSurroundingRank> GetUserSurroundingRank(string playerId)
        {
            try
            {
                var result = await ConnectionManager.GetAsync<DataType<List<UserSurroundingRankRow[]>>>(ConnectionManager.Api.UserSurroundingRank + playerId);
                return new UserSurroundingRank(playerId)
                {
                    UsersRankDaily = result.data[0].ToList(),
                    UsersRankWeekly = result.data[1].ToList(),
                    UsersRankMontly = result.data[2].ToList()
                };
            }
            catch
            {
                return null;
            }
        }

        public void Dispose()
        {
            return;
        }
    }
}
