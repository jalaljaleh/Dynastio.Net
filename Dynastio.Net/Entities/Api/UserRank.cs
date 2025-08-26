using System;
using System.Collections.Generic;
using System.Linq;

namespace Dynastio.Net
{
    /// <summary>
    /// Represents a user's rank standings over different time periods
    /// (daily, weekly, monthly) within Dynast.io.
    /// </summary>
    public class UserRank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRank"/> class.
        /// </summary>
        /// <param name="data">
        /// A list of integer rank positions:
        /// <list type="bullet">
        ///   <item><description>Index 0 → Daily rank</description></item>
        ///   <item><description>Index 1 → Weekly rank</description></item>
        ///   <item><description>Last index → Monthly rank</description></item>
        /// </list>
        /// A negative value indicates no valid ranking and will be converted to 99999.
        /// </param>
        public UserRank(List<int> data)
        {
            _data = data;
        }

        /// <summary>
        /// Backing data for ranks. Kept immutable after construction.
        /// </summary>
        private readonly List<int> _data;

        /// <summary>
        /// Daily ranking position. Returns 99999 if no valid rank.
        /// </summary>
        public int Daily => CheckValue(_data.FirstOrDefault());

        /// <summary>
        /// Weekly ranking position. Returns 99999 if no valid rank.
        /// </summary>
        public int Weekly => CheckValue(_data.Skip(1).FirstOrDefault());

        /// <summary>
        /// Monthly ranking position. Returns 99999 if no valid rank.
        /// </summary>
        public int Monthly => CheckValue(_data.LastOrDefault());

        /// <summary>
        /// Helper method: if rank value is negative, substitute with 99999
        /// to represent "unranked" or "max rank placeholder".
        /// </summary>
        private int CheckValue(int value) => value < 0 ? 99999 : value;
    }
}
