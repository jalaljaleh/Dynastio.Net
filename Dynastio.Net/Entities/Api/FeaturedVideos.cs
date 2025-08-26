using Newtonsoft.Json;
using System;

namespace Dynastio.Net
{
    /// <summary>
    /// Represents a featured video entry in Dynast.io,
    /// including display priority, content details, and expiration date.
    /// </summary>
    public class FeaturedVideos
    {
        /// <summary>
        /// Determines the order of display. 
        /// Lower numbers may indicate higher priority.
        /// </summary>
        [JsonProperty("priority")]
        public int Priority { get; set; }

        /// <summary>
        /// Unique identifier for the video entry.
        /// </summary>
        [JsonProperty("_id")]
        public string Id { get; set; }

        /// <summary>
        /// Direct URL link to the video content.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// URL or identifier for the video's texture/thumbnail image.
        /// </summary>
        [JsonProperty("texture")]
        public string Texture { get; set; }

        /// <summary>
        /// Group or category this video belongs to.
        /// Useful for organizing featured videos by event or theme.
        /// </summary>
        [JsonProperty("group")]
        public string Group { get; set; }

        /// <summary>
        /// Date and time when the video should no longer be featured.
        /// </summary>
        [JsonProperty("expireAt")]
        public DateTime ExpireAt { get; set; }

        /// <summary>
        /// Internal version number for this record (used by the backend).
        /// </summary>
        [JsonProperty("__v")]
        public int V { get; set; }
    }
}
