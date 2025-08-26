namespace Dynastio.Net
{
    /// <summary>
    /// Defines the different categories of server listings available in Dynast.io.
    /// Used for filtering or selecting which servers to query.
    /// </summary>
    public enum ServerType
    {
        /// <summary>
        /// Only public servers, showing only their top players.
        /// </summary>
        PublicServersWithTopPlayers,

        /// <summary>
        /// Only public servers, showing all players connected.
        /// </summary>
        PublicServersWithAllPlayers,

        /// <summary>
        /// All servers (public + private), showing only their top players.
        /// </summary>
        AllServersWithTopPlayers,

        /// <summary>
        /// All servers (public + private), showing every player connected.
        /// </summary>
        AllServersWithAllPlayers
    }
}
