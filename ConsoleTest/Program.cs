






using Dynastio.Net;

DynastioApi client = new DynastioApi("sdf:Sdfs");


var s = client.OnlinePlayers;
var ss = client.OnlineServers;
var sf = client.OnlineServersWithPlayers;
var sg = client.OnlineTopPlayers;


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