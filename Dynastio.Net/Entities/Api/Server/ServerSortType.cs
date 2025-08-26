namespace Dynastio.Net
{
    /// <summary>
    /// Specifies the ways a server list can be sorted
    /// when retrieved or displayed in Dynast.io.
    /// </summary>
    public enum ServerSortType
    {
        /// <summary>
        /// Default sorting — usually by server priority or natural order.
        /// </summary>
        Default,

        /// <summary>
        /// Sort servers in descending order based on the top player’s score.
        /// </summary>
        DescendingByPlayerScore
    }
}
