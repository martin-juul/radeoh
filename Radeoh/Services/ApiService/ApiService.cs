﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Radeoh.Services.ApiService
{
    public class ApiService : IDisposable, IApiService
    {
        public static readonly MediaTypeHeaderValue ApplicationJson =
            MediaTypeHeaderValue.Parse("application/json");

        public static readonly MediaTypeWithQualityHeaderValue ApplicationJsonQ =
            MediaTypeWithQualityHeaderValue.Parse("application/json");

        private Uri _baseUri;
        private HttpClient _client;

        public Action<HttpRequestMessage> ConfigureHttpRequest { get; set; }

        /// <summary>
        /// Cancellation token source. It can be used to cancel send operation.
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

        /// <summary>
        /// Gets the underlying <see cref="HttpClient"/> client.
        /// </summary>
        public HttpClient Client
        {
            get => EnsureHttpClient();
            set => _client = value;
        }


        /// <summary>
        /// Constructs the service with a custom HttpClient
        /// </summary>
        /// <param name="httpClient">The external HttpClient.</param>
        public ApiService(HttpClient httpClient)
        {
            Client = httpClient ?? throw new NullReferenceException(nameof(httpClient));
            _disposedValue = true;
        }

        /// <summary>
        /// Constructs an instance based on a baseurl and message delegate.
        /// </summary>
        /// <param name="baseUri">The base Uri to the WEB API service</param>
        /// <param name="configureHttpRequestMessage">The optional conflagration delegate for the request message.</param>
        public ApiService(Uri baseUri, Action<HttpRequestMessage> configureHttpRequestMessage = null)
        {
            _baseUri = baseUri;
            ConfigureHttpRequest = configureHttpRequestMessage;
        }

        /// <inheritdoc cref="IApiService"/>
        private HttpClient EnsureHttpClient()
        {
            if (_client != null) return _client;
            _client = new HttpClient
            {
                BaseAddress = _baseUri
            };

            return _client;
        }

        /// <inheritdoc cref="IApiService"/>
        public Task<HttpResponseMessage> SendJsonRequest<T>(HttpMethod method, Uri uri, T json)
        {
            var serialized = string.Empty;
            if (json == null) return SendJsonRequest(method, uri, serialized);
            
            serialized = ApiServiceExtensions.GetJsonString(json);

            return SendJsonRequest(method, uri, serialized);
        }

        /// <inheritdoc cref="IApiService"/>
        public Task<HttpResponseMessage> SendJsonRequest(HttpMethod method, Uri uri, string json)
        {
            var client = EnsureHttpClient();
            var request = new HttpRequestMessage();

            request.Headers.Accept.Add(ApplicationJsonQ);
            request.Method = method;
            request.RequestUri = uri;
            if (method != HttpMethod.Get)
            {
                request.Content = new StringContent(json, Encoding.UTF8, ApplicationJson.MediaType);
            }

            //request.Content.Headers.ContentType = ApplicationJson;
            ConfigureHttpRequest?.Invoke(request);
            return client.SendAsync(request, CancellationTokenSource.Token);
        }

        #region IDisposable Support

        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Client?.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}