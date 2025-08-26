using Newtonsoft.Json;

namespace Dynastio.Net
{
    /// <summary>
    /// Represents a player's rank within the Dynast.io Discord community.
    /// Can be used to track leaderboard positions or special Discord-based roles.
    /// </summary>
    public class DiscordRank
    {
        /// <summary>
        /// Initializes a new <see cref="DiscordRank"/> with the given rank value.
        /// </summary>
        /// <param name="rank">The player's Discord rank number.</param>
        public DiscordRank(int rank)
        {
            Rank = rank;
        }

        /// <summary>
        /// The player's rank number in Discord-related leaderboards or systems.
        /// </summary>
        [JsonProperty("rank")]
        public int Rank { get; set; }
    }
}
