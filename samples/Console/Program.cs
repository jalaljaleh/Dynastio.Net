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

            var servers = client.Game.OnlineServers;

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
            var client = new DynastioClient("Dynastio_Api_Key");

            var profile = await client.Database.GetUserProfileAsync("discord:0000000000000000").TryGet();

        }

    }

}
