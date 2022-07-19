using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    internal class ConnectionManager
    {
        internal DynastioProviderConfiguration Api;
        internal HttpClient client;
        public ConnectionManager(DynastioProviderConfiguration api)
        {
            if (api == null) return;
            Api = api;
        }
        public void Initialize()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            client = new HttpClient(clientHandler);
            client.BaseAddress = new Uri(Api.BaseAddress);
            client.DefaultRequestHeaders.Add(Api.AuthorizationKey, Api.AuthorizationValue);
        }
        internal async Task<string> GetAsync(string api)
        {
            var data = await client.GetStringAsync(api);
            return await Task.FromResult(data);
        }
        internal async Task<Type> GetAsync<Type>(string api)
        {
            var data = await GetAsync(api);
            var obj = JsonConvert.DeserializeObject<Type>(data);
            return await Task.FromResult(obj);
        }
        public void Dispose()
        {
            client.Dispose();
        }
    }
}
