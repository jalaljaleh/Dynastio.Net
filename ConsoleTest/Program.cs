






using Dynastio.Net;

DynastioApi client = new DynastioApi("key:value)");


var s = client.OnlineTopPlayers;
var t = client.OnlineServers;

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