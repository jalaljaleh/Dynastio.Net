using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using System.Diagnostics;


namespace Dynastio.Net
{
    public static class PlayerExtentions
    {
        public static bool IsDiscordAuth(this Player player)
        {
            return player.IsAuth && player.Id.Contains("discord");
        }
        public static bool ServiceEquals(this Player player, string service)
        {
            if (service == "none" || string.IsNullOrEmpty(service)) return true;
            return service == "guest" ? !player.IsAuth : player.IsAuth && player.Id.Contains(service);
        }
    }

}
