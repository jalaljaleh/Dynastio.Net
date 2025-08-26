using Newtonsoft.Json;
using System.Collections.Generic;

namespace Dynastio.Net
{
    /// <summary>
    /// Represents detailed player statistics in Dynast.io,
    /// tracking various in-game actions and survival metrics.
    /// </summary>
    public class PlayerStat
    {
        /// <summary>
        /// Number of kills by entity type.
        /// Key = <see cref="EntityType"/>, Value = kill count.
        /// </summary>
        [JsonProperty("kill")]
        public Dictionary<EntityType, int> Kill { get; set; } = new();

        /// <summary>
        /// Number of build actions by entity type.
        /// Key = <see cref="EntityType"/>, Value = number built.
        /// </summary>
        [JsonProperty("build")]
        public Dictionary<EntityType, int> Build { get; set; } = new();

        /// <summary>
        /// Number of deaths caused by each entity type.
        /// Key = <see cref="EntityType"/>, Value = death count.
        /// </summary>
        [JsonProperty("death")]
        public Dictionary<EntityType, int> Death { get; set; } = new();

        /// <summary>
        /// Amount of resources gathered by item type.
        /// Key = <see cref="ItemType"/>, Value = gather count.
        /// </summary>
        [JsonProperty("gather")]
        public Dictionary<ItemType, int> Gather { get; set; } = new();

        /// <summary>
        /// Number of items crafted by type.
        /// Key = <see cref="ItemType"/>, Value = craft count.
        /// </summary>
        [JsonProperty("craft")]
        public Dictionary<ItemType, int> Craft { get; set; } = new();

        /// <summary>
        /// Number of items bought from the shop by type.
        /// Key = <see cref="ItemType"/>, Value = quantity purchased.
        /// </summary>
        [JsonProperty("shop")]
        public Dictionary<ItemType, int> Shop { get; set; } = new();

        /// <summary>
        /// Number of self-inflicted deaths (suicides).
        /// </summary>
        [JsonProperty("suicide")]
        public int Suicide { get; set; }

        /// <summary>
        /// Total number of in-game days the player has survived.
        /// </summary>
        [JsonProperty("days_survived")]
        public int SurvivedDays { get; set; }
    }
}
