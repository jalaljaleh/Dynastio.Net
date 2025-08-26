using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    /// <summary>
    /// Provides utility methods for calculating game-related values
    /// such as level-based coin rewards.
    /// </summary>
    public static class DynastioApiHelper
    {
        /// <summary>
        /// The maximum level a player can reach in Dynast.io.
        /// Used to normalize reward scaling.
        /// </summary>
        public const int MaxLevel = 40;

        /// <summary>
        /// Calculates the coin reward for a given player level.
        /// The reward increases exponentially with level,
        /// starting small and accelerating toward MaxLevel.
        /// </summary>
        /// <param name="level">The player's current level (1 to MaxLevel).</param>
        /// <returns>The number of coins awarded for reaching that level.</returns>
        public static double GetLevelCoinsReward(int level)
        {
            // Normalize level to a 0–1 scale
            double b = 1.0 / MaxLevel;

            // Scaling factor to ensure the reward reaches ~10,000 at MaxLevel
            double a = 10000 / (Math.Exp(1) - 1);

            // Apply exponential growth formula and round to nearest whole number
            return Math.Round(a * (Math.Exp(b * level) - 1));
        }
    }
}
