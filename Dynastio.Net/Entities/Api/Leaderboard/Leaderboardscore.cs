using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dynastio.Net
{
    /// <summary>
    /// Identifiers for the various leaderboard categories and timeframes.
    /// Used to select the correct leaderboard from a collection.
    /// </summary>
    public enum LeaderboardScoreItemId
    {
        // General leaderboards
        Day,
        Week,
        Month,

        // Solo mode leaderboards
        Solo_Day,
        Solo_Week,
        Solo_Month,

        // PvP mode leaderboards
        PVP_Day,
        PVP_Week,
        PVP_Month
    }

    /// <summary>
    /// Represents a leaderboard grouping (e.g., "day_sum", "pvp.week_sum") with its related scores.
    /// </summary>
    public class LeaderboardScore
    {
        /// <summary>
        /// The string identifier for the leaderboard type (e.g., "day_sum").
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Array of leaderboard entries for this leaderboard type.
        /// </summary>
        [JsonProperty("data")]
        public LeaderboardScoreItem[] Data { get; set; }

        /// <summary>
        /// Retrieves a leaderboard from a collection based on the given <paramref name="type"/>.
        /// Returns null if no matching leaderboard is found.
        /// </summary>
        public static LeaderboardScore GetLeaderboard(List<LeaderboardScore> leaderboards, LeaderboardScoreItemId type)
        {
            if (leaderboards == null || leaderboards.Count == 0)
                return null;

            return type switch
            {
                LeaderboardScoreItemId.Day => leaderboards.FirstOrDefault(a => a.Id == "day_sum"),
                LeaderboardScoreItemId.Week => leaderboards.FirstOrDefault(a => a.Id == "week_sum"),
                LeaderboardScoreItemId.Month => leaderboards.FirstOrDefault(a => a.Id == "month_sum"),

                LeaderboardScoreItemId.PVP_Day => leaderboards.FirstOrDefault(a => a.Id == "pvp.day_sum"),
                LeaderboardScoreItemId.PVP_Week => leaderboards.FirstOrDefault(a => a.Id == "pvp.week_sum"),
                LeaderboardScoreItemId.PVP_Month => leaderboards.FirstOrDefault(a => a.Id == "pvp.month_sum"),

                LeaderboardScoreItemId.Solo_Day => leaderboards.FirstOrDefault(a => a.Id == "solo.day_sum"),
                LeaderboardScoreItemId.Solo_Week => leaderboards.FirstOrDefault(a => a.Id == "solo.week_sum"),
                LeaderboardScoreItemId.Solo_Month => leaderboards.FirstOrDefault(a => a.Id == "solo.month_sum"),

                _ => leaderboards.FirstOrDefault(a => a.Id == "month_sum")
            };
        }
    }

    /// <summary>
    /// Represents an individual leaderboard entry (player result) within a given leaderboard type.
    /// </summary>
    public class LeaderboardScoreItem
    {
        /// <summary>
        /// Unique identifier for the leaderboard entry (e.g., player ID).
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The date and time when this entry was created.
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// The player's display name for this leaderboard entry.
        /// </summary>
        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        /// <summary>
        /// The score value for this leaderboard entry.
        /// </summary>
        [JsonProperty("score")]
        public long Score { get; set; }

        /// <summary>
        /// The date and time when this entry was last updated.
        /// </summary>
        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
