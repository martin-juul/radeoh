using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Radeoh.Services.ApiService
{
    public interface IApiService
    {
        /// <summary>
        /// Underlying <see cref="HttpClient" client= />
        /// </summary>
        HttpClient Client { get; set; }

        /// <summary>
        /// Send a json request
        /// </summary>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        /// <param name="json"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<HttpResponseMessage> SendJsonRequest<T>(HttpMethod method, Uri uri, T json);

        /// <summary>
        /// Send raw json string
        /// </summary>
        /// <param name="method">The standard HTTP method.</param>
        /// <param name="uri">endpoint.</param>
        /// <param name="json">The JSON string to send.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> task object representing the asynchronous operation.</returns>
        Task<HttpResponseMessage> SendJsonRequest(HttpMethod method, Uri uri, string json);
    }
}