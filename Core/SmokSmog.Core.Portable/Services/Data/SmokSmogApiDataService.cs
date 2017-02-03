using Newtonsoft.Json.Linq;
using SmokSmog.Extensions;
using SmokSmog.Model;
using SmokSmog.Net.Http;
using SmokSmog.Services.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SmokSmog.Services.Data
{
    public class SmokSmogApiDataProvider : RestDataProviderBase
    {
        private readonly IStorageService _settingsService;

        public SmokSmogApiDataProvider(IHttpClient httpClient, IStorageService settingsService)
            //#if DEBUG
            //            : base(httpClient, "http://beta-api.smoksmog.jkostrz.name")
            //#else
            : base(httpClient, "http://api.smoksmog.jkostrz.name")
        //#endif
        {
            if (settingsService == null)
                throw new ArgumentNullException(nameof(settingsService));

            _settingsService = settingsService;
        }

        public override Guid Id { get; } = new Guid("2A0E0002-CDD2-484F-A4DA-2B2973D8BC33");

        public override string Name => "SmokSmog REST API";

        private string language
            => (_settingsService.LanguageCode?.ToLowerInvariant()?.Substring(0, 2) ?? "en").Equals("pl") ? "pl" : "en";

        public override async Task<List<Measurement>> GetMeasurementsAsync(Model.Station station, IEnumerable<Parameter> parameters, CancellationToken cancellationToken)
        {
            try
            {
                if (station == null)
                    throw new ArgumentNullException(nameof(station));

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
                    new Uri(BaseUri, $"{language}/stations/{station.Id}"));
                request.Headers.Add("X-Smog-AdditionalMeasurements", "pm25");

                Task<string> task = SendAsync(request, cancellationToken);
                string response = await task;
                var token = JToken.Parse(response);
                var particulates = token["particulates"];

                var parametersLocal = parameters.ToList();

                List<Measurement> measurements = new List<Measurement>();
                foreach (var item in particulates)
                {
                    var id = item["id"].Value<int?>();
                    if (!id.HasValue) continue;

                    var parameter = parametersLocal.FirstOrDefault(p => p.Id == id.Value);
                    if (parameter == null) continue;

                    var dateString = item["date"]?.Value<string>();
                    DateTime date;
                    DateTime.TryParse(dateString, out date);

                    measurements.Add(new Measurement(station, parameter)
                    {
                        Aggregation = AggregationType.Avg1Hour,
                        Value = item["value"].Value<double?>(),
                        Date = date,
                    });

                    if (parameter.Type != ParameterType.PM25 && parameter.Type != ParameterType.C6H6)
                    {
                        var avg = item["avg"]?.Value<double?>();
                        if (avg.HasValue)
                        {
                            measurements.Add(new Measurement(station, parameter)
                            {
                                Aggregation = AggregationType.Avg24Hour,
                                Value = avg.Value,
                                Date = date,
                            });
                        }
                    }
                }

                return measurements;
            }
            catch (Exception ex)
            {
                Diagnostics.Logger.Log(ex);
                throw;
            }
        }

        /// <summary>
        /// </summary>
        /// <see cref="http://api.smoksmog.jkostrz.name/en/stations/4"/>
        /// <param name="station">          </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<List<Parameter>> GetParametersAsync(Model.Station station, CancellationToken cancellationToken)
        {
            try
            {
                if (station == null)
                    throw new ArgumentNullException(nameof(station));

                // TODO
                //X-Smog-AdditionalMeasurements pm25

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
                    new Uri(BaseUri, $"{language}/stations/{station.Id}"));
                request.Headers.Add("X-Smog-AdditionalMeasurements", "pm25");

                //Task<string> task = GetStringAsync($"{language}/stations/{station.Id}", cancellationToken);
                Task<string> task = SendAsync(request, cancellationToken);

                string response = await task;
                var token = JToken.Parse(response);
                var particulates = token["particulates"];

                List<Parameter> parameters = new List<Parameter>();
                foreach (var item in particulates)
                {
                    var id = item["id"].Value<int?>();
                    if (!id.HasValue) continue;

                    var norm = item["norm"].Value<double?>();
                    var normAggregation = AggregationType.Avg24Hour;

                    var shortName = (item["short_name"].Value<string>() ?? "")?.RemoveWhiteSpaces()?.Trim();
                    if (shortName == "PM\u2082.\u2085")
                    {
                        shortName = "PM\u2082\u200A\u05c5\u200A\u2085";
                    }

                    var parameter = new Parameter(station, id.Value)
                    {
                        Name = (item["name"].Value<string>() ?? "")?.RemoveWhiteSpaces()?.Trim(),
                        ShortName = shortName,
                        Unit = (item["unit"].Value<string>() ?? "")?.RemoveWhiteSpaces()?.Trim(),
                    };

                    if (parameter.Type == ParameterType.PM25 || parameter.Type == ParameterType.C6H6)
                    {
                        normAggregation = AggregationType.Avg1Year;
                    }

                    if (norm.HasValue)
                    {
                        parameter.Norm = new Norm()
                        {
                            Aggregation = normAggregation,
                            Value = norm.Value,
                            Name = "WIOŚ"
                        };
                    }

                    parameters.Add(parameter);
                }

                return parameters;
            }
            catch (Exception ex)
            {
                Diagnostics.Logger.Log(ex);
                throw;
            }
        }

        public override async Task<Station> GetStationAsync(int id, CancellationToken token)
        {
            if (id <= 0) return null;
            var list = await GetStationsAsync(token);
            return list.FirstOrDefault(o => o.Id == id);
        }

        public override async Task<List<Station>> GetStationsAsync(CancellationToken cancellationToken)
        {
            var stations = new List<Station>();

            try
            {
                Task<string> stationTask = GetStringAsync($"{language}/stations", cancellationToken);
                Task<string> provincesTask = GetStringAsync($"{language}/provinces", cancellationToken);

                string stationResponse = await stationTask;
                var stationsJArray = JArray.Parse(stationResponse);
                // manual parse stations
                foreach (var item in stationsJArray)
                {
                    int? id = item["id"].Value<int?>();
                    if (!id.HasValue || id <= 0) continue;

                    var station = new Station(id.Value)
                    {
                        Name = (item["name"].Value<string>() ?? "").RemoveWhiteSpaces().Trim(),
                        Geocoordinate = new Geocoordinate()
                        {
                            Latitude = item["lat"].Value<double?>() ?? 0d,
                            Longitude = item["lon"].Value<double?>() ?? 0d
                        },
                    };
                    stations.Add(station);
                }

                string provincesResponse = await provincesTask;
                var provincesJArray = JArray.Parse(provincesResponse);
                // get provinces information for stations
                foreach (var province in provincesJArray)
                {
                    string name = province["name"].Value<string>()?.RemoveWhiteSpaces().Trim();
                    var stationsInProvince = province["stations"].Values<JToken>();

                    if (stationsInProvince == null || string.IsNullOrWhiteSpace(name))
                        continue;

                    foreach (var station in stationsInProvince)
                    {
                        int id = station["id"].Value<int?>() ?? -1;

                        var st = (from s in stations where s.Id == id select s).FirstOrDefault();
                        if (st != null)
                            st.Province = name;
                    }
                }

                cancellationToken.ThrowIfCancellationRequested();

                return stations;
            }
            catch (Exception ex)
            {
                Diagnostics.Logger.Log(ex);
                throw;
            }
        }
    }
}