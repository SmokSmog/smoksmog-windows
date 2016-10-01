using System;
using System.Collections.Generic;
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
            //Models.Stations data = null;

            var stations = new List<Model.Station>();

            try
            {
                string response = await GetStringAsync(language + "/stations");
                response = response.Replace("null", "0");

                var s = JArray.Parse(response);

                foreach (var item in s)
                {
                    var station = new Model.Station()
                    {
                        Id = item["id"].Value<int>(),
                        Name = item["name"].Value<string>(),
                        Geocoordinate = new Geocoordinate()
                        {
                            Latitude = item["lat"].Value<double>(),
                            Longitude = item["lon"].Value<double>()
                        },
                    };
                    stations.Add(station);
                }

                //data.Sort(
                //    delegate (Models.Station p1, Models.Station p2)
                //    {
                //        return String.Compare(p1.name, p2.name);
                //    }
                //);

                return stations;
            }
            catch
            {
                throw;
            }
        }
    }
}