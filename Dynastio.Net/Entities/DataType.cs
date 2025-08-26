using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Dynastio.Net
{
    /// <summary>
    /// Generic wrapper for API responses from Dynast.io.
    /// Encapsulates metadata (status, error info, code) and the main payload (`data`).
    /// </summary>
    /// <typeparam name="T">
    /// The type of the payload inside the API response.
    /// For example: Player, MatchResult, or any DTO.
    /// </typeparam>
    internal class DataType<T>
    {
        /// <summary>
        /// Indicates if the API returned an error.
        /// `true` means the request failed or was invalid.
        /// </summary>
        [JsonProperty("error")]
        public bool Error { get; set; } = false;

        /// <summary>
        /// If <see cref="Error"/> is true, contains the server-supplied error message.
        /// Otherwise, null.
        /// </summary>
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; } = null;

        /// <summary>
        /// Numeric status or error code provided by the API.
        /// Often used for internal error handling or custom logic.
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; } = 0;

        /// <summary>
        /// The main payload of the response, strongly typed as <typeparamref name="T"/>.
        /// </summary>
        [JsonProperty("data")]
        public T Data;

        /// <summary>
        /// The textual status of the request (e.g., "success", "fail").
        /// May mirror the HTTP status in string form.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; } = null;

        /// <summary>
        /// A list of game server objects returned by the API, if applicable.
        /// Virtual so it can be overridden in subclasses.
        /// Null if the API call does not return server data.
        /// </summary>
        [JsonProperty("servers")]
        public virtual List<Server> Servers { get; set; } = null;
    }
}
