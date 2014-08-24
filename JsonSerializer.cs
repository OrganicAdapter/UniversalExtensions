using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalExtensions
{
    public class JsonSerializer
    {
        public async static Task<T> Deserialize<T>(string json)
        {
            return await JsonConvert.DeserializeObjectAsync<T>(json);
        }

        public async static Task<string> Serialize<T>(List<T> list)
        {
            return await JsonConvert.SerializeObjectAsync(list);
        }

        public async static Task<string> Serialize(object obj)
        {
            return await JsonConvert.SerializeObjectAsync(obj);
        }
    }
}
