using Newtonsoft.Json;

namespace Dynastio.Net
{
    /// <summary>
    /// Represents version information for the Dynast.io client or API.
    /// Contains the current version identifier and a download URL for updates.
    /// </summary>
    public class Version
    {
        /// <summary>
        /// The current release version string (e.g., "1.4.2").
        /// Mapped from the JSON property "version".
        /// </summary>
        [JsonProperty("version")]
        public string CurrentVersion { get; set; }

        /// <summary>
        /// The URL where the latest version can be downloaded.
        /// Mapped from the JSON property "url".
        /// </summary>
        [JsonProperty("url")]
        public string DownloadUrl { get; set; }
    }
}
