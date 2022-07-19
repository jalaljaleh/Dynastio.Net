
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    public static class DynastioExtensions
    {
        public static Server FirstOrDefaultByName(this ICollection<Server> servers, string name)
        {
            var _server = servers.Where(a => a.ServerName.ToLower() == name.ToLower()).FirstOrDefault();
            return _server ?? servers.Where(a => a.ServerName.ToLower().Contains(name.ToLower())).FirstOrDefault();
        }

    }
}
