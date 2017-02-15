using System;
using System.Threading;
using System.Threading.Tasks;

#if  WINDOWS_UWP || WINDOWS_APP || WINDOWS_PHONE_APP

using Windows.Web.Http;

#endif

namespace SmokSmog.Services.Network
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(Uri uri);

        Task<HttpResponseMessage> GetAsync(Uri uri, CancellationToken token);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token);
    }
}