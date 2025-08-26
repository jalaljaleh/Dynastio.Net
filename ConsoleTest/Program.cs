






using Dynastio.Net;

DynastioApi client = new DynastioApi("token");


var s = client.FeaturedVideos;
var t = client.LeaderboardScores;
var m = await client.GetUserProfileAsync("discord:567315648531791872");

//List<Server> servers;
//try
//{
//    servers = client.OnlineServers;
//}
//catch
//{
//    servers = client.GetServersAsync(ServerType.AllServersWithTopPlayers).GetAwaiter().GetResult();

//}

Console.WriteLine("");