using Newtonsoft.Json;
using System;

namespace Dynastio.Net
{
    /// <summary>
    /// Represents a player currently in a Dynast.io server,
    /// storing identity, position, stats, and authentication state.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Parameterless constructor for manual instantiation
        /// or JSON deserialization.
        /// </summary>
        public Player() { }

        /// <summary>
        /// Updates this player object with a reference to the server it belongs to
        /// and regenerates a unique hash-based identifier.
        /// </summary>
        /// <param name="parent">The parent server instance.</param>
        /// <returns>The updated <see cref="Player"/> instance (for chaining).</returns>
        internal Player Update(Server parent)
        {
            Parent = parent;
            UniqueId = "hash$" + InternalId.GetHashCode();
            return this;
        }

        /// <summary>
        /// Checks whether the player's searchable nickname contains
        /// the provided string (case-insensitive, trimmed).
        /// </summary>
        /// <param name="nickname">The nickname to search for.</param>
        public bool IsMatched(string nickname) =>
            SearchableNickname.Contains(nickname.ToLower().Trim());

        // -------------------
        // Non-serialized props
        // -------------------

        /// <summary>
        /// A generated internal unique ID for in-memory identification.
        /// Not serialized to JSON.
        /// </summary>
        [JsonIgnore]
        public string UniqueId { get; set; } = string.Empty;

        /// <summary>
        /// Lowercase, search-friendly version of <see cref="Nickname"/>.
        /// Not serialized to JSON.
        /// </summary>
        [JsonIgnore]
        public string SearchableNickname { get; set; } = string.Empty;

        /// <summary>
        /// A sanitized version of <see cref="Nickname"/> safe for display.
        /// Not serialized to JSON.
        /// </summary>
        [JsonIgnore]
        public string SafeNickname { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the player has any valid authentication ID.
        /// </summary>
        [JsonIgnore]
        public bool IsAuth => !string.IsNullOrEmpty(Id);

        /// <summary>
        /// Indicates whether the player is authenticated via Discord.
        /// </summary>
        [JsonIgnore]
        public bool IsDiscordAuth => IsAuth && Id.Contains("discord:");

        /// <summary>
        /// Reference to the parent <see cref="Server"/> object this player belongs to.
        /// Not serialized to JSON.
        /// </summary>
        [JsonIgnore]
        public Server Parent { get; set; }


        // -------------------
        // Serialized props
        // -------------------

        /// <summary>
        /// Internal unique identifier of the player (server-assigned).
        /// </summary>
        [JsonProperty("internal_id")]
        public ulong InternalId { get; set; } = 0;

        /// <summary>
        /// Current X coordinate of the player in the game world.
        /// </summary>
        [JsonProperty("x")]
        public int X { get; set; }

        /// <summary>
        /// Current Y coordinate of the player in the game world.
        /// </summary>
        [JsonProperty("y")]
        public int Y { get; set; }

        /// <summary>
        /// The display nickname of the player.
        /// </summary>
        [JsonProperty("name")]
        public string Nickname { get; set; }

        /// <summary>
        /// The player's authentication or identity string (may include platform info).
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The player's current score.
        /// </summary>
        [JsonProperty("score")]
        public long Score { get; set; }

        /// <summary>
        /// The player's current level.
        /// </summary>
        [JsonProperty("level")]
        public int Level { get; set; }

        /// <summary>
        /// The name of the team the player belongs to (if any).
        /// </summary>
        [JsonProperty("team")]
        public string Team { get; set; } = string.Empty;
    }
}
