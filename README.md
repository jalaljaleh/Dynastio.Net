<div id="top"></div>

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="#">
    <img src="Dynastio.Net/Resource/logo.jpg" alt="Logo" width="150" height="150">
  </a>

  <h3 align="center">Dynastio.Net</h3>

 <p align="center">
    Dynastio.Net is an unofficial .NET API Wrapper for the Dynast.io client https://dynast.io.
    <br />
    <br />
    <a href="https://discord.gg/GVUXMNv7vV"><strong>Join our discord server »</strong></a>
    <br />
    <a href="https://github.com/jalaljaleh/Dynastio.Net/issues">Report Bug</a>
    ·
    <a href="https://github.com/jalaljaleh/Dynastio.Net/issues">Request Feature</a>
  </p>
</div>

<div align="center">
  
[![Discord](https://discord.com/api/guilds/875716592770637824/widget.png)](https://discord.gg/x5j4cZtnWR) 
</div>


<!-- Road Map -->
## Roadmap
- [X] Cache provider
- [X] Nightly provider

<p align="right">(<a href="#top">back to top</a>)</p>


<!-- GETTING STARTED -->
## Getting Started
- Meet the prerequisites.


### Prerequisites
This is an example of how to list things you need to use the software and how to install them.

- .Net46 or .Net 5
- Dynast.io Api Token


### Usage
```csharp
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        public async Task MainAsync()
        {
            DynastioClient client = new DynastioClient("Your_Api_Token");
            
            var onlineServers = await client.Game.GetOnlineServers();
            Console.WriteLine("Servers: {0} Servers are online.", onlineServers.Count);
            Console.WriteLine("\n");
            Console.WriteLine("\n");

            var singapore = onlineServers.Find(a => a.ServerName.Equals("singapore-0"));
            Console.WriteLine("Top player name: {0}", singapore.TopPlayerName);
            Console.WriteLine("Top player score: {0}", singapore.TopPlayerScore);
            Console.WriteLine("Top player level: {0}", singapore.TopPlayerLevel);
            Console.WriteLine("Server lifetime: {0}", singapore.Lifetime);

            var personalChest = await client.Database.GetUserPersonalchestAsync("discord:805534924622004274");
            Console.WriteLine("{0,-15} {1,5}","Item","Count");
            Console.WriteLine("-----------------------");
            personalChest.Items.ForEach(a => Console.WriteLine(string.Format("{0,-15} {1,-5}", a.ItemType, a.Count)));
            
            var version = await client.Game.GetCurrentVersionAsync();
            Console.WriteLine("Current version: {0}", version.CurrentVersion);
            Console.WriteLine("\n");
            Console.WriteLine("\n");
```


<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#top">back to top</a>)</p>

