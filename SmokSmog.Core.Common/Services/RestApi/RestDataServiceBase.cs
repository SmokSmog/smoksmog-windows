using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SmokSmog.Model;
using SmokSmog.Services.Storage;

namespace SmokSmog.Services.RestApi
{
    public abstract class RestDataServiceBase : IDataService
    {
        protected Uri BaseUri { get; }

        protected ISettingsService SettingsService { get; }

        protected HttpClient HttpClient { get; }

        protected RestDataServiceBase(ISettingsService settingsService, string baseUrl)
            : this(settingsService, new Uri(baseUrl))
        { }

        protected RestDataServiceBase(ISettingsService settingsService, Uri baseUri)
        {
            BaseUri = baseUri;
            Debug.Assert(baseUri != null && !string.IsNullOrWhiteSpace(BaseUri.AbsolutePath));

            SettingsService = settingsService;
            Debug.Assert(SettingsService != null);

            HttpClient = new HttpClient();
            Debug.Assert(HttpClient != null);
        }

        public abstract string Name { get; }

        public abstract List<StationState> GetStationInfo(IEnumerable<int> stationIds);

        public abstract StationState GetStationInfo(int stationId);

        public abstract List<StationState> GetStationInfoAll();

        public abstract IEnumerable<Measurement> GetStationMeasurements(int stationId);

        public abstract Task<IEnumerable<Measurement>> GetStationMeasurementsAsync(int stationId);

        public abstract IEnumerable<Parameter> GetStationParticulates(int stationId);

        public abstract Task<IEnumerable<Parameter>> GetStationParticulatesAsync(int stationId);

        public abstract IEnumerable<Station> GetStations();

        public abstract Task<IEnumerable<Station>> GetStationsAsync();

        protected Task<string> GetStringAsync(string relativeUri) => GetStringAsync(new Uri(relativeUri, UriKind.Relative));

        protected async Task<string> GetStringAsync(Uri relativeUri)
        {
            try
            {
                HttpResponseMessage message = await HttpClient.GetAsync(new Uri(BaseUri, relativeUri));

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