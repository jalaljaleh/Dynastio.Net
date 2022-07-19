

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    internal class DynastioProvider : IDynastioProvider
    {
        private DynastioProviderConfiguration Configuration { get; set; }
        public ISocketGame Game { get; set; }
        public ISocketDatabase Database { get; set; }
        public string Name { get; set; }
        public bool IsMain { get; set; }

        internal DynastioProvider(DynastioProviderConfiguration api)
        {
            Configuration = api;
            Game = new SocketGame(this, api);
            Database = new SocketDatabase(this, api);
            Name = api.Name;
            IsMain = api.Name == "Main";
        }
        public void Dispose()
        {
            Game.Dispose();
            Database.Dispose();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
