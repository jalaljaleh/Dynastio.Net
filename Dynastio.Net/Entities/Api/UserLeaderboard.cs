using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dynastio.Net
{
    /// <summary>
    /// Represents a single entry in a surrounding rank list — a player’s ID,
    /// nickname, score, and timestamps for when the entry was created or updated.
    /// </summary>
    public class UserSurroundingRankRow
    {
        /// <summary>
        /// Unique identifier of the player.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Date and time when this rank entry was created.
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The player’s display name.
        /// </summary>
        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        /// <summary>
        /// Player's score for the ranking period.
        /// </summary>
        [JsonProperty("score")]
        public int Score { get; set; }

        /// <summary>
        /// Date and time when this rank entry was last updated.
        /// </summary>
        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// Represents the surrounding ranks of a specific player in daily, weekly, and monthly leaderboards.
    /// </summary>
    public class UserSurroundingRank
    {
        /// <summary>
        /// Creates a new instance tied to the given owner/player ID.
        /// </summary>
        /// <param name="ownerId">The player’s unique ID.</param>
        public UserSurroundingRank(string ownerId)
        {
            OwnerId = ownerId;
        }

        /// <summary>
        /// The ID of the player for whom the surrounding ranks are being tracked.
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        /// The current player's daily rank entry from the daily leaderboard.
        /// Returns null if the player is not found in <see cref="UsersRankDaily"/>.
        /// </summary>
        public UserSurroundingRankRow Daily =>
            UsersRankDaily?.FirstOrDefault(a => a.Id == OwnerId);

        /// <summary>
        /// The current player's weekly rank entry from the weekly leaderboard.
        /// Returns null if the player is not found in <see cref="UsersRankWeekly"/>.
        /// </summary>
        public UserSurroundingRankRow Weekly =>
            UsersRankWeekly?.FirstOrDefault(a => a.Id == OwnerId);

        /// <summary>
        /// The current player's monthly rank entry from the monthly leaderboard.
        /// Returns null if the player is not found in <see cref="UsersRankMonthly"/>.
        /// </summary>
        public UserSurroundingRankRow Monthly =>
            UsersRankMonthly?.FirstOrDefault(a => a.Id == OwnerId);

        /// <summary>
        /// List of rank rows for the daily leaderboard.
        /// </summary>
        public List<UserSurroundingRankRow> UsersRankDaily { get; set; } = new();

        /// <summary>
        /// List of rank rows for the weekly leaderboard.
        /// </summary>
        public List<UserSurroundingRankRow> UsersRankWeekly { get; set; } = new();

        /// <summary>
        /// List of rank rows for the monthly leaderboard.
        /// </summary>
        public List<UserSurroundingRankRow> UsersRankMonthly { get; set; } = new();
    }
}
