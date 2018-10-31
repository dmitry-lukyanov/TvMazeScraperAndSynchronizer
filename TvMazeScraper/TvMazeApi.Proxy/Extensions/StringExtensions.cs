using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace TvMazeApi.Proxy.Extensions
{
    public static class StringExtensions
    {
        public static T ParseTo<T>(this string obj)
        {
            var token = JToken.Parse(obj);
            return token.ToObject<T>(JsonSerializer.Create(SerializationSettings));
        }

        private static JsonSerializerSettings SerializationSettings => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
        };
    }
}
