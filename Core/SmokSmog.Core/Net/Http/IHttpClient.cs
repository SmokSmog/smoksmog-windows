using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace SmokSmog.Net.Http
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(Uri uri);

        Task<HttpResponseMessage> GetAsync(Uri uri, CancellationToken token);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token);
    }
}