using System;
using System.Threading.Tasks;
using Dynastio.Net;
namespace Console
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var client = new DynastioApi("token:key");

            var changelog = client.Changelog;
            var gameVersion = client.Version;

        }
        public async Task MainAsync()
        {


        }

    }

}
