using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Radeoh.Services.ApiService
{
    public static class ApiServiceExtensions
    {
        /// <summary>
        /// Extracts the JSON object from the HTTP response message.
        /// </summary>
        /// <typeparam name="T">The expected type of the response object.</typeparam>
        /// <param name="res">The HTTP response message including the status code and data.</param>
        /// <returns>The deserialized object of the type.</returns>
        public static T DeserializeJsonResponse<T>(this HttpResponseMessage res)
        {
            var respStr = res.Content.ToString();
            return !string.IsNullOrWhiteSpace(respStr) ? GetJsonObject<T>(respStr) : default(T);
        }

        /// <summary>
        /// Asynchronously extracts the JSON object from the HTTP response message.
        /// </summary>
        /// <typeparam name="T">The expected type of the response object.</typeparam>
        /// <param name="res">The HTTP response message including the status code and data.</param>
        /// <returns>The deserialized object of the type.</returns>
        public static async Task<T> DeserializeJsonResponseAsync<T>(this HttpResponseMessage res)
        {
            var respStr = await res.Content.ReadAsStringAsync();
            return !string.IsNullOrWhiteSpace(respStr) ? GetJsonObject<T>(respStr) : default(T);
        }

        /// <summary>
        /// Extracts the JSON object from the string.
        /// </summary>
        /// <typeparam name="T">The expected type of the response object.</typeparam>
        /// <param name="json">The serialized object string.</param>
        /// <returns>The deserialized object of the type.</returns>
        public static T GetJsonObject<T>(string json)
        {
            using (var str = new StringReader(json))
            using (var jsonReader = new JsonTextReader(str))
            {
                var serializer = new JsonSerializer();
                var res = (T) serializer.Deserialize(jsonReader, typeof(T));
                return res;
            }
        }

        /// <summary>
        /// Serialize the object into the JSON string.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="json">The instance of the object.</param>
        /// <returns>The serialized string.</returns>
        public static string GetJsonString<T>(T json)
        {
            string serialized;
            using (var stringWriter = new StringWriter())
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(stringWriter, json);
                serialized = stringWriter.ToString();
            }

            return serialized;
        }
    }
}