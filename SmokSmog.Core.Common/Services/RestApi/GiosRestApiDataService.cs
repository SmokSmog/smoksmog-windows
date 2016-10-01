using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmokSmog.Model;
using SmokSmog.Services.Storage;

namespace SmokSmog.Services.RestApi
{
    public class GiosRestApiDataService : RestDataServiceBase
    {
        public GiosRestApiDataService(ISettingsService settingsService) :
            base(settingsService, @"http://powietrze.gios.gov.pl/pjp/rest/")
        {
        }

        public override string Name => "GIOS REST API";

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
            return GetStationsAsync().Result;
        }

        public override async Task<IEnumerable<Station>> GetStationsAsync()
        {
            string responseText = await GetStringAsync("station/findAll");

            List<Model.Station> lista = new List<Model.Station>();
            //JObject list = JObject.Parse(responseText);

            var list = JsonConvert.DeserializeObject<List<JToken>>(responseText);

            //System.Diagnostics.Debugger.Break();

            foreach (var item in list)
            {
                //"id":114,
                //"stationName":"Wrocław - Bartnicza",
                //"gegrLat":"51.115933",
                //"gegrLon":"17.141125",
                //"city":{
                //              "id":1064,
                //   "name":"Wrocław",
                //   "commune":{
                //      "communeName":"Wrocław",
                //      "districtName":"Wrocław",
                //      "provinceName":"DOLNOŚLĄSKIE"
                //   }
                //          },
                //"addressStreet":"ul. Bartnicza"
                try
                {
                    lista.Add(new Station()
                    {
                        Name = item["stationName"].ToString(),
                        Geocoordinate = new Geocoordinate()
                        {
                            Longitude = (double)item["gegrLon"],
                            Latitude = (double)item["gegrLat"]
                        },
                        City = (item["city"]["name"] ?? item["city"]["commune"]["communeName"]).ToString(),
                        Province = (item["city"]["commune"]["provinceName"] ?? "-").ToString().ToLower(),
                        Address = (item["addressStreet"]).ToString(),
                        //Aqi = (int)item["id"] % 7,
                    });
                }
                catch (Exception)
                {
#if (DEBUG)
                    //e.ToString();
                    //System.Diagnostics.Debugger.Break();
#endif
                }
            }
            return lista;
        }
    }
}