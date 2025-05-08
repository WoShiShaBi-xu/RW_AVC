using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RW.Framework.Extensions
{
    public static class JsonExtension
    {
        public static string ToJson(this object obj, JsonSerializerSettings? settings = null)
        {
            settings ??= new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return JsonConvert.SerializeObject(obj, settings);
        }

        public static T? ToObject<T>(this string str, JsonSerializerSettings? settings = null)
        {
            settings ??= new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return JsonConvert.DeserializeObject<T>(str, settings);
        }
    }
}
