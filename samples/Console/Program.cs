using System;
using System.Threading.Tasks;
using Dynastio.Net;
namespace Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //new Program().MainAsync().GetAwaiter().GetResult();


            var client = new DynastioClient();

            var servers = client.Main.OnlineServers;

            foreach (var server in servers)
            {
                System.Console.WriteLine(server.Label);
                foreach (var player in server.Players)
                {
                    System.Console.WriteLine(player.Nickname);
                    System.Console.WriteLine(player.Score);
                }
            }


        }
        public async Task MainAsync()
        {
            var client = new DynastioClient("Api_Key_Value");
            var changelog = client.Main.ChangeLog;
            var videos = await client.Main.GetFeaturedVideosAsync();
            var profileDev = await client["nightly"].GetUserProfileAsync("google:112093004829153652749");
            var profileZhaleh = await client[DynastioProviderType.Main].GetUserProfileAsync("discord:805534924622004274");
           
        }

    }

}
