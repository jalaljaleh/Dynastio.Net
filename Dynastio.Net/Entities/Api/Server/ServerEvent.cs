using Newtonsoft.Json;
using System;

namespace Dynastio.Net
{
    /// <summary>
    /// Represents an event taking place on a server.
    /// </summary>
    public class ServerEvent
    {
        [JsonProperty("id")]
        /// <summary>
        /// Unique identifier for the event.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Short title or heading for the event.
        /// </summary>
        [JsonProperty("header")]
        public string Header { get; set; }

        /// <summary>
        /// Detailed description of the event.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Date and time when the event starts.
        /// </summary>
        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Date and time when the event ends.
        /// </summary>
        [JsonProperty("finish_time")]
        public DateTime FinishTime { get; set; }

        /// <summary>
        /// Additional details about the type of event.
        /// </summary>
        [JsonProperty("kind")]
        public Kind Kind { get; set; }
    }

    /// <summary>
    /// Defines the type and characteristics of an event.
    /// </summary>
    public class Kind
    {
        /// <summary>
        /// Type of event (e.g., "PvP", "BossFight", "SpecialDrop").
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Coefficient or multiplier representing the event's effect.
        /// </summary>
        [JsonProperty("coef")]
        public uint Coef { get; set; }
    }
}
