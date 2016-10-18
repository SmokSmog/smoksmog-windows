using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SmokSmog.Services.Storage;

namespace SmokSmog.Services.Data
{
    public abstract class RestDataProviderBase : AsyncDataProviderBase
    {
        protected RestDataProviderBase(ISettingsService settingsService, string baseUrl)
            : this(settingsService, new Uri(baseUrl))
        { }

        protected RestDataProviderBase(ISettingsService settingsService, Uri baseUri)
        {
            BaseUri = baseUri;
            Debug.Assert(baseUri != null && !string.IsNullOrWhiteSpace(BaseUri.AbsolutePath));

            SettingsService = settingsService;
            Debug.Assert(SettingsService != null);

            HttpClient = new HttpClient();
            Debug.Assert(HttpClient != null);
        }

        protected Uri BaseUri { get; }
        protected HttpClient HttpClient { get; }
        protected ISettingsService SettingsService { get; }

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
            catch
            {
                throw;
            }
        }
    }
}