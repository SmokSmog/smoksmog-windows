using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SmokSmog.Model;
using SmokSmog.Services.Storage;

namespace SmokSmog.Services.Data
{
    public class SmokSmogApiDataProvider : RestDataProviderBase
    {
        public SmokSmogApiDataProvider(ISettingsService settingsService)
            : base(settingsService, "http://api.smoksmog.jkostrz.name")
        {
        }

        public override Guid Id { get; } = new Guid("2A0E0002-CDD2-484F-A4DA-2B2973D8BC33");

        public override string Name => "SmokSmog REST API";

        private string language
            => (SettingsService?.LanguageCode?.ToLowerInvariant() ?? "en").Equals("pl") ? "pl" : "en";

        public override Task<IEnumerable<Measurement>> GetMeasurementsAsync(int stationId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<Parameter>> GetParticulatesAsync(int stationId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<Station>> GetStationsAsync(CancellationToken cancellationToken)
        {
            var stations = new List<Station>();

            try
            {
                Task<string> stationTask = GetStringAsync(language + "/stations", cancellationToken);
                Task<string> provincesTask = GetStringAsync(language + "/provinces", cancellationToken);

                string stationResponse = await stationTask;
                var stationsJArray = JArray.Parse(stationResponse);
                // manual parse stations
                foreach (var item in stationsJArray)
                {
                    var station = new Station()
                    {
                        Id = item["id"].Value<int?>() ?? 0,
                        Name = Regex.Replace(item["name"].Value<string>() ?? "", " {2,}", " ").Trim(),
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
                    string name = province["name"].Value<string>()?.Trim();
                    var stationsInProvince = province["stations"].Values<JToken>();

                    if (stationsInProvince == null || string.IsNullOrWhiteSpace(name))
                        continue;

                    name = Regex.Replace(name, " {2,}", " ");

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
            catch
            {
                throw;
            }
        }
    }
}