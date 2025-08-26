using Newtonsoft.Json;

namespace Dynastio.Net
{
    /// <summary>
    /// Represents a single item stored in a player's personal chest
    /// within Dynast.io, including quantity, durability, and ownership data.
    /// </summary>
    public class PersonalChestItem
    {
        /// <summary>
        /// The slot index or position of this item in the chest.
        /// </summary>
        [JsonProperty("index")]
        public int Index { get; set; }

        /// <summary>
        /// The type of the item stored in this chest slot.
        /// </summary>
        public ItemType ItemType { get; set; }

        /// <summary>
        /// The number of items in this stack.
        /// </summary>

        public int Count { get; set; }

        /// <summary>
        /// The remaining durability of the item (0 = broken, max = new).
        /// </summary>
        public int Durability { get; set; }

        /// <summary>
        /// The unique identifier of the player who owns this chest item.
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        /// A unique token associated with the item (for server validation or identification).
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Additional descriptive details or metadata about the item.
        /// </summary>
        public string Details { get; set; }
    }
}
