using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastio.Net
{
    public static class TaskExtensions
    {
        public static async Task<T> TryGet<T>(this Task<T> task)
        {
            try
            {
                return await task;
            }
            catch
            {
                return default(T);
            }
        }
    }
    
}
