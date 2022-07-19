
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    internal class DataType<T>
    {
        [JsonProperty("error")] public bool Error { get; set; }
        [JsonProperty("errorMessage")] public string ErrorMessage { get; set; }
        [JsonProperty("code")] public int Code { get; set; }
        [JsonProperty("data")] public T data;
        public T Value
        {
            get
            {
                if (Error)
                {
                    throw new Exception($"Error Code {Code}", new Exception(ErrorMessage));
                }
                return data;
            }
            set
            {
                data = value;
            }
        }

        [JsonProperty("status")] public string status { get; set; }
        [JsonProperty("servers")] public virtual List<Server> Servers { get; set; }


    }
}
