using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dynastio.Net
{
    /// <summary>
    /// Represents a Dynastio game server with metadata, players, and events.
    /// </summary>
    public class Server
    {
        // Identification & Access

        /// <summary>Unique peer key, if available.</summary>
        [JsonProperty("peer_key")]
        public long? PeerKey { get; set; }

        /// <summary>Server IPv4 or hostname.</summary>
        [JsonProperty("ip")]
        public string Ip { get; set; } = string.Empty;

        /// <summary>Plain-text port for legacy clients.</summary>
        [JsonProperty("port")]
        public int Port { get; set; }

        /// <summary>Hostname for SSL connections.</summary>
        [JsonProperty("ssl_host")]
        public string SslHost { get; set; } = string.Empty;

        /// <summary>Port for SSL connections.</summary>
        [JsonProperty("ssl_port")]
        public int SslPort { get; set; }

        /// <summary>Port used for SSL ping checks.</summary>
        [JsonProperty("ssl_ping_port")]
        public int SslPingPort { get; set; }


        // Server Status

        /// <summary>Current number of connected players.</summary>
        [JsonProperty("client_count")]
        public int PlayersCount { get; set; }

        /// <summary>Maximum simultaneous connections allowed.</summary>
        [JsonProperty("connections_limit")]
        public int ConnectionsLimit { get; set; }

        /// <summary>Average load across server threads.</summary>
        [JsonProperty("load_avg")]
        public int LoadAvg { get; set; }

        /// <summary>Peak load observed.</summary>
        [JsonProperty("load_max")]
        public int LoadMax { get; set; }

        /// <summary>Total runtime in seconds.</summary>
        [JsonProperty("lifetime")]
        public int Lifetime { get; set; }

        /// <summary>Server timestamp (Unix ms).</summary>
        [JsonProperty("server_time")]
        public double? ServerTime { get; set; }


        // Gameplay & Map

        /// <summary>Name of the current map.</summary>
        [JsonProperty("map")]
        public string Map { get; set; } = string.Empty;

        /// <summary>Hash or checksum of the map resources.</summary>
        [JsonProperty("map_hash")]
        public string MapHash { get; set; } = string.Empty;

        /// <summary>Identifier of the game mode.</summary>
        [JsonProperty("game_mode")]
        public string GameMode { get; set; } = string.Empty;

        /// <summary>True if this is a custom game mode.</summary>
        [JsonProperty("custom_mode")]
        public bool CustomMode { get; set; }


        // Administrative

        /// <summary>Label or friendly name of the server.</summary>
        [JsonProperty("label")]
        public string Label { get; set; } = string.Empty;

        /// <summary>Geographical region (e.g., "eu-west").</summary>
        [JsonProperty("region")]
        public string Region { get; set; } = string.Empty;

        /// <summary>Version string of the running server build.</summary>
        [JsonProperty("version")]
        public string Version { get; set; } = string.Empty;

        /// <summary>Secret token for private or cross-server invitations.</summary>
        [JsonProperty("secret")]
        public string Secret { get; set; } = string.Empty;

        /// <summary>True if the server is private/invite-only.</summary>
        [JsonProperty("private")]
        public bool IsPrivate { get; set; }


        // Top Player Snapshot

        /// <summary>Display name of the highest-scoring player.</summary>
        [JsonProperty("top_player_name")]
        public string TopPlayerName { get; set; } = string.Empty;

        /// <summary>Score achieved by the top player.</summary>
        [JsonProperty("top_player_score")]
        public long TopPlayerScore { get; set; }

        /// <summary>Level of the top player.</summary>
        [JsonProperty("top_player_level")]
        public int TopPlayerLevel { get; set; }


        // Miscellaneous

        /// <summary>Number of dropped frames since start.</summary>
        [JsonProperty("frame_drop")]
        public int FrameDrop { get; set; }

        /// <summary>Backend identifier for routing or diagnostics.</summary>
        [JsonProperty("backend")]
        public string Backend { get; set; } = string.Empty;

        /// <summary>True if using the new I/O subsystem.</summary>
        [JsonProperty("new_io")]
        public bool NewIo { get; set; }


        // Collections

        /// <summary>Players currently on this server.</summary>
        [JsonProperty("players")]
        public List<Player> Players { get; set; } = new();

        /// <summary>Recent server-side events (join/leave/etc.).</summary>
        [JsonProperty("events")]
        public List<ServerEvent> Events { get; set; } = new();


        // Utility Methods

        /// <summary>
        /// Determines whether the server label contains the specified term (case-insensitive).
        /// </summary>
        /// <param name="term">Substring to match in the label.</param>
        public bool IsMatched(string term) =>
            !string.IsNullOrWhiteSpace(term)
            && Label.IndexOf(term.Trim(), StringComparison.OrdinalIgnoreCase) >= 0;

        /// <summary>
        /// Generates a direct-play link: https://dynast.io/?direct=ip:port
        /// </summary>
        public Uri DirectLink =>
            new UriBuilder("https", "dynast.io") { Query = $"direct={Ip}:{Port}" }.Uri;

        /// <summary>
        /// Returns a read-only list of players, each updated with this server context.
        /// </summary>
        public IReadOnlyList<Player> GetPlayers() =>
            Players.Count > 0
                ? Players.Select(p => p.Update(this)).ToList()
                : Array.Empty<Player>();

        /// <summary>
        /// Provides a concise description of the server’s current state.
        /// </summary>
        public override string ToString() =>
            $"{Label} [{Region}] – {PlayersCount}/{ConnectionsLimit} players on “{Map}”";
    }
}
