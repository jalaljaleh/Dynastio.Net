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
  
  <a href="https://www.nuget.org/packages/Dynastio.Net/">
    <img src="https://img.shields.io/nuget/vpre/Dynastio.Net.svg?maxAge=2592000?style=plastic" alt="NuGet">
  </a>
 
   <a href="https://discord.gg/GVUXMNv7vV">
    <img src="https://discord.com/api/guilds/875716592770637824/widget.png" alt="Discord">
  </a>
  
</div>


<!-- Road Map -->
## Roadmap
- [X] Cache provider
- [X] Nightly provider

<p align="right">(<a href="#top">back to top</a>)</p>


<!-- GETTING STARTED -->
## Getting Started
- .Net46 or .Net 5
- Dynast.io Api Token

### Usage

#### Get Online Servers
```csharp
            var client = new DynastioClient();
            var servers = await client.Main.GetOnlineServersAsync();

            foreach (var server in servers)
            {
                System.Console.WriteLine(server.Label);
            }
            // frankfurt-01
            // london-01
            // london-02
            // singapore-01
            // ..
```

#### Nightly Servers
```csharp
            var client = new DynastioClient();
            var servers = await client.Nightly.GetOnlineServersAsync();
            // nightly-01
            // nightly-02
            // nightly-solo
```

#### User Profile & Api Authorization 
```csharp
            var client = new DynastioClient("Api_Key_Value");
            var profile = await client.Nightly.GetUserProfileAsync("discord:805534924622004274");
```

#### Custom Provider
```csharp
            var client = new DynastioClient("Api_Key_Value", false);
            var config = new DynastioProviderConfiguration()
            {
                Name = "Main",
                BaseAddress = "https://auth.dynast.io/",
                // Fill All...
            };
            client.AddProvider(config);
```

#### Custom Cache
```chsarp
            var cacheConfig = new DynastioCacheConfiguration()
            {
                CacheTimeChangeLog = 500, //ms
                CacheTimeServers = 30000, //ms
                ...
            };
            var client = new DynastioClient("Api_Key_Value", true,cacheConfig);
```

#### Get Providers
```csharp
            var client = new DynastioClient();
            var nightly01 = client.Nightly;
            var nightly02 = client["nightly"]; // provider name
            var nightly03 = client[DynastioProviderType.Nightly];

            var main01 = client.Main;
            var main02 = client["main"]; // provider name
            var main03 = client[DynastioProviderType.Main];

            var changelog = nightly02.ChangeLog;
```

Look at the samples for more [Samples](https://github.com/jalaljaleh/Dynastio.Net/tree/master/samples)


<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#top">back to top</a>)</p>

