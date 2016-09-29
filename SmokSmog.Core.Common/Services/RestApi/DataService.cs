using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmokSmog.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmokSmog.Services.RestApi
{
    public class DataService : IDataService
    {
        private HttpClient httpClient = new HttpClient();

        public ICollection<Measurement> GetStationMeasurements(int stationId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Parameter> GetStationParticulates(int stationId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Station> GetStations()
        {
            return GetStationsAsync().Result;

            //return new List<Model.Station>()
            //    {
            //        new Model.Station() { Name="Kraków - Ditla", City="Kraków" },
            //        new Model.Station() { Name="Kraków - Bronowice", City="Kraków" },
            //        new Model.Station() { Name="Kraków - Aleja Kraśińskiego", City="Kraków" },
            //        new Model.Station() { Name="Kraków - Nowa Huta", City="Kraków" },
            //        new Model.Station() { Name="Kraków - Bierzanów", City="Kraków" },
            //        new Model.Station() { Name="Tarnów", City="Tarnów" },
            //        new Model.Station() { Name="Nowy Sącz", City="Nowy Sącz" },
            //        new Model.Station() { Name="Warszawa", City="Warszawa" },
            //    };
        }

        public async Task<ICollection<Station>> GetStationsAsync()
        {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@"http://powietrze.gios.gov.pl/pjp/rest/station/findAll"),
                Method = HttpMethod.Get
            };
            HttpResponseMessage response = await httpClient.SendAsync(request);
            string responseText = await response.Content.ReadAsStringAsync();

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

        public Task<ICollection<Parameter>> GetStationParticulatesAsync(int stationId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Measurement>> GetStationMeasurementsAsync(int stationId)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Measurement> IDataService.GetStationMeasurements(int stationId)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Measurement>> IDataService.GetStationMeasurementsAsync(int stationId)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Parameter> IDataService.GetStationParticulates(int stationId)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Parameter>> IDataService.GetStationParticulatesAsync(int stationId)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Station> IDataService.GetStations()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Station>> IDataService.GetStationsAsync()
        {
            throw new NotImplementedException();
        }

        public StationState GetStationInfo(int stationId)
        {
            throw new NotImplementedException();
        }

        public List<StationState> GetStationInfo(IEnumerable<int> stationIds)
        {
            throw new NotImplementedException();
        }

        public List<StationState> GetStationInfoAll()
        {
            throw new NotImplementedException();
        }
    }
}