using System;
using Dynastio.Net;
namespace Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new DynastioClient("Your_Api_Token");
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
    }
}
