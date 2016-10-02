using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SmokSmog.Model;
using SmokSmog.Services.Storage;

namespace SmokSmog.Services.RestApi
{
    public class SmokSmogApiDataService : RestDataServiceBase
    {
        public SmokSmogApiDataService(ISettingsService settingsService)
            : base(settingsService, "http://api.smoksmog.jkostrz.name")
        {
        }

        public override string Name => "SmokSmog REST API";

        private string language
            => (SettingsService?.LanguageCode?.ToLowerInvariant() ?? "en").Equals("pl") ? "pl" : "en";

        public override StationState GetStationInfo(int stationId)
        {
            throw new NotImplementedException();
        }

        public override List<StationState> GetStationInfo(IEnumerable<int> stationIds)
        {
            throw new NotImplementedException();
        }

        public override List<StationState> GetStationInfoAll()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Measurement> GetStationMeasurements(int stationId)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<Measurement>> GetStationMeasurementsAsync(int stationId)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Parameter> GetStationParticulates(int stationId)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<Parameter>> GetStationParticulatesAsync(int stationId)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Station> GetStations()
        {
            IEnumerable<Station> result = new Station[0];
            try
            {
                result = GetStationsAsync().Result;
            }
            finally
            {
            }
            return result;
        }

        public override async Task<IEnumerable<Station>> GetStationsAsync()
        {
            var stations = new List<Station>();

            try
            {
                Task<string> stationTask = GetStringAsync("/stations");
                Task<string> provincesTask = GetStringAsync(language + "/provinces");

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

                return stations;
            }
            catch
            {
                throw;
            }
        }
    }
}