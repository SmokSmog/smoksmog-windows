using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace SmokSmog.Net.Http
{
    public class HttpClientProxy : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public HttpClientProxy(HttpClient httpClient)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            return await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseContentRead);
        }

        public async Task<HttpResponseMessage> GetAsync(Uri uri, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                throw new TaskCanceledException();

            var task = _httpClient.GetAsync(uri, HttpCompletionOption.ResponseContentRead);
            return await task;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await _httpClient.SendRequestAsync(request, HttpCompletionOption.ResponseContentRead);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                throw new TaskCanceledException();

            var task = _httpClient.SendRequestAsync(request, HttpCompletionOption.ResponseContentRead);
            return await task;
        }
    }
}