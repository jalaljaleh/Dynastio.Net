using System;
using System.Threading.Tasks;
using Dynastio.Net;
namespace Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();

            var client = new DynastioClient();

            var nightly01 = client.Nightly;
            var nightly02 = client["nightly"]; // provider name
            var nightly03 = client[DynastioProviderType.Nightly];

            var main01 = client.Main;
            var main02 = client["main"]; // provider name
            var main03 = client[DynastioProviderType.Main];

            var changelog = nightly02.ChangeLog;
        }
        public async Task MainAsync()
        {
            var client = new DynastioClient("Api_Key_Value");


            // Nightly Players (we refresh players every 30s)
            // based on ServerType.AllServersWithAllPlayers
            var players = client[DynastioProviderType.Nightly].OnlinePlayers;
            //var players = client["nightly"].GetOnlinePlayersAsync(ServerType.AllServersWithAllPlayers);

            // get fresh players list
            players = await client.Nightly.GetOnlinePlayersAsync(ServerType.AllServersWithAllPlayers);
            foreach (var player in players)
            {
                System.Console.WriteLine(player.Nickname);
                System.Console.WriteLine(player.Score);
                System.Console.WriteLine(player.Level);
                System.Console.WriteLine(player.Team);
            }

            // Main Servers
            var servers = client[DynastioProviderType.Main].OnlineServers;

            // Nightly User Profile
            var profileDev = await client["nightly"].GetUserProfileAsync("google:112093004829153652749");

            // Personal Chest
            var profileZhaleh = await client[DynastioProviderType.Main].GetUserPersonalchestAsync("discord:805534924622004274");

        }

    }

}
