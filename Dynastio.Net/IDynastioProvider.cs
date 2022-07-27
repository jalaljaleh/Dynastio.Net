using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    public interface IDynastioProvider : ISocketDatabase, ISocketGame, IDisposable
    {
        string ProviderName { get; }
        bool IsMainProvider { get; }
        bool IsNightlyProvider { get; }
    }
}
