using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dynastio.Net
{
    /// <summary>
    /// Represents a player's complete profile in Dynast.io, including
    /// currency, progression, unlocks, and activity data.
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// Default constructor for JSON deserialization or manual creation.
        /// </summary>
        public Profile() { }

        /// <summary>
        /// The number of coins currently owned by the player.
        /// </summary>
        [JsonProperty("coins")]
        public int Coins { get; set; }

        /// <summary>
        /// Collection of badges earned by the player.
        /// </summary>
        [JsonProperty("badges")]
        public List<BadgeType> Badges { get; set; } = new();

        /// <summary>
        /// The last server the player connected to.
        /// </summary>
        [JsonProperty("latest_server")]
        public string LatestServer { get; set; }

        /// <summary>
        /// The timestamp of the player's last activity.
        /// </summary>
        [JsonProperty("last_active_at")]
        public DateTime LastActiveAt { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Recipes the player has unlocked.
        /// </summary>
        [JsonProperty("unlocked_recipes")]
        public List<ItemType> UnlockedRecipes { get; set; } = new();

        /// <summary>
        /// Skins the player has unlocked.
        /// </summary>
        [JsonProperty("unlocked_skins")]
        public List<SkinType> UnlockedSkins { get; set; } = new();

        /// <summary>
        /// Buildings the player has unlocked.
        /// </summary>
        [JsonProperty("unlocked_buildings")]
        public List<EntityType> UnlockedBuildings { get; set; } = new();

        /// <summary>
        /// Total amount of experience points accumulated by the player.
        /// </summary>
        [JsonProperty("experience")]
        public int Experience { get; set; }

        /// <summary>
        /// Current level of the player.
        /// </summary>
        [JsonProperty("level")]
        public int Level { get; set; }

        /// <summary>
        /// Calculates the maximum experience required for the current level.
        /// </summary>
        public float GetExperienceMax() =>
            250 + Level * 500;

        /// <summary>
        /// Calculates the proportional experience based on an arbitrary value.
        /// Example: determining progress toward a milestone.
        /// </summary>
        /// <param name="value">The reference value to calculate against.</param>
        public float GetExperience(int value) =>
            GetExperience() * value / GetExperienceMax();

        /// <summary>
        /// Calculates how much experience is needed to reach the next level.
        /// </summary>
        public float GetRequireExperienceForNewLevel() =>
            GetExperienceMax() - GetExperience();

        /// <summary>
        /// Returns the player's current experience points.
        /// </summary>
        public float GetExperience() =>
            Experience;
    }
}
