using Newtonsoft.Json;

namespace Dynastio.Net
{
    /// <summary>
    /// Represents a complete profile card for a player,
    /// including profile information, chest details, and player statistics.
    /// </summary>
    public class ProfileCard
    {
        /// <summary>
        /// The player's profile information.
        /// </summary>
        [JsonProperty("profile")]
        public Profile Profile { get; set; }

        /// <summary>
        /// The player's personal chest data (inventory, storage, etc.).
        /// </summary>
        [JsonProperty("chest")]
        public PersonalChest Chest { get; set; }

        /// <summary>
        /// The player's in-game statistics.
        /// </summary>
        [JsonProperty("stat")]
        public PlayerStat Stat { get; set; }
    }

    /// <summary>
    /// Represents the same profile card structure,
    /// but with some properties stored as raw JSON strings instead of objects.
    /// </summary>
    internal class ProfileCardEntity
    {
        /// <summary>
        /// The player's profile information.
        /// </summary>
        [JsonProperty("profile")]
        public Profile Profile { get; set; }

        /// <summary>
        /// The player's chest data as a raw JSON string.
        /// Useful when chest content is not deserialized into <see cref="Personalchest"/>.
        /// </summary>
        [JsonProperty("chest")]
        public string Chest { get; set; }

        /// <summary>
        /// The player's stats as a raw JSON string.
        /// Useful when stats content is not deserialized into <see cref="PlayerStat"/>.
        /// </summary>
        [JsonProperty("stat")]
        public string Stat { get; set; }
    }
}
