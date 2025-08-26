using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    // Represents a team entity in the Dynastio.Net domain
    public class Team
    {
        // Unique identifier for the team
        public string Id { get; set; }

        // Human-readable name of the team
        public string Name { get; set; }

        // Reference to the server this team belongs to
        public Server Server { get; set; }

        // Number of members currently in the team
        // Tip: Could be computed from Players.Count to avoid mismatches
        public int MembersCount { get; set; }

        // Collection of Player objects that are part of this team
        public List<Player> Players { get; set; }
    }
}
