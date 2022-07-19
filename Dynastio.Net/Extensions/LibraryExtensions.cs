using Dynastio.Net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    public static class LibraryExtensions
    {
        internal static Output DeserializeObjectData<Input, Output>(this DataType<Input> data)
        {
            var _data = JsonConvert.DeserializeObject(data.Value.ToString()).ToString();
            return JsonConvert.DeserializeObject<Output>(_data);
        }


    }
}
