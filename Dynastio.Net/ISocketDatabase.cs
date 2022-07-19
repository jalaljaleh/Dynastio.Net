
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    public interface ISocketDatabase : IDisposable
    {
        Task<List<Leaderboardcoin>> GetCoinLeaderboardAsync();
        Task<List<Leaderboardscore>> GetScoreLeaderboardAsync(LeaderboardType leaderboardType);
        Task<List<Leaderboardscore>> GetScoreLeaderboardAsync();
        Task<PlayerStat> GetUserStatAsync(string playerId);
        Task<Profile> GetUserProfileAsync(string playerId);
        Task<Personalchest> GetUserPersonalchestAsync(string PlayerId);
        Task<ProfileDetails> GetUserProfileDetailsAsync(string playerId);
        Task<UserRank> GetUserRank(string playerId);
        Task<UserSurroundingRank> GetUserSurroundingRank(string playerId);
        Task<bool> IsUserAccountExistAsync(string Id);
        List<Leaderboardcoin> Leaderboardcoins { get; }
        List<Leaderboardscore> Leaderboardscores { get; }
    }
}
