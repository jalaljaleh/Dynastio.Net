using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    public interface IDynastioProvider : IDisposable
    {
        ISocketGame Game { get; set; }
        ISocketDatabase Database { get; set; }
        string Name { get; set; }
        bool IsMain { get; set; }
    }
}
