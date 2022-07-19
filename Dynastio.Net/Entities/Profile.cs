using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    public class Profile
    {
        [JsonProperty("coins")]
        public int Coins;

        [JsonProperty("badges")]
        public List<BadgeType> Badges;

        [JsonProperty("latest_server")]
        public string LatestServer;

        [JsonProperty("last_active_at")]
        public DateTime LastActiveAt;
        public ProfileDetails Details { get; set; }

        public float GetExperienceMax()
        {
            return 250 + Details.Level * 500;
        }
        public float GetExperience(int value)
        {
            return GetExperience() * value / GetExperienceMax();
        }
        public float GetRequireExperienceForNewLevel()
        {
            return GetExperienceMax() - GetExperience();
        }
        public float GetExperience()
        {
            return Details.Experience;
        }
    }

}
