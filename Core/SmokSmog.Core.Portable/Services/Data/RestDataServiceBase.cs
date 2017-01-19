using SmokSmog.Net.Http;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SmokSmog.Services.Data
{
    public abstract class RestDataProviderBase : AsyncDataProviderBase
    {
        protected RestDataProviderBase(IHttpClient httpClient, string baseUrl)
            : this(httpClient, new Uri(baseUrl))
        { }

        protected RestDataProviderBase(IHttpClient httpClient, Uri baseUri)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            HttpClient = httpClient;

            if (baseUri == null)
                throw new ArgumentNullException(nameof(baseUri));

            BaseUri = baseUri;
            Debug.Assert(baseUri != null && !string.IsNullOrWhiteSpace(BaseUri.AbsolutePath));

            HttpClient.BaseAddress = BaseUri;
        }

        protected Uri BaseUri { get; }

        protected IHttpClient HttpClient { get; }

        protected async Task<string> GetStringAsync(string relativeUri)
            => await GetStringAsync(new Uri(relativeUri, UriKind.Relative));

        protected async Task<string> GetStringAsync(string relativeUri, CancellationToken cancellationToken)
            => await GetStringAsync(new Uri(relativeUri, UriKind.Relative), cancellationToken);

        protected async Task<string> GetStringAsync(Uri relativeUri)
            => await GetStringAsync(relativeUri, new CancellationToken());

        protected virtual async Task<string> GetStringAsync(Uri relativeUri, CancellationToken cancellationToken)
        {
            try
            {
                HttpResponseMessage message = await HttpClient.GetAsync(new Uri(BaseUri, relativeUri), cancellationToken);

                if (message.IsSuccessStatusCode && (
                    message.StatusCode == HttpStatusCode.OK ||
                    message.StatusCode == HttpStatusCode.NotModified))
                {
                    return await message.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new HttpRequestException($"Code:{message.StatusCode} : {message.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Diagnostics.Logger.Log(ex);
                throw;
            }
        }
    }
}