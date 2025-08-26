using Newtonsoft.Json;
using System;

namespace Dynastio.Net
{
    /// <summary>
    /// Represents a single leaderboard entry tracking coin counts for a player.
    /// </summary>
    public class LeaderboardCoin
    {
        /// <summary>
        /// Unique identifier for the player.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Display name of the player.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Total number of coins owned by the player.
        /// </summary>
        [JsonProperty("coins")]
        public ulong Coins { get; set; }

        /// <summary>
        /// UTC date and time when the player was last active.
        /// </summary>
        [JsonProperty("last_active_at")]
        public DateTime LastActiveAt { get; set; }

        /// <summary>
        /// Name or identifier of the most recent server the player joined.
        /// </summary>
        [JsonProperty("latest_server")]
        public string LatestServer { get; set; }
    }
}
