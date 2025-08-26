namespace Dynastio.Net
{
    /// <summary>
    /// Represents a player's profile details in Dynast.io,
    /// including experience points and current level.
    /// </summary>
    public class ProfileDetails
    {
        /// <summary>
        /// The total amount of experience points (XP) the player has earned.
        /// This value is typically used to calculate the <see cref="Level"/>.
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        /// The player's current level based on accumulated <see cref="Experience"/>.
        /// </summary>
        public int Level { get; set; }
    }
}
