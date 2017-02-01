using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmokSmog.Model;
using SmokSmog.Net.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmokSmog.Services.Data
{
    public class GiosRestApiDataService : RestDataProviderBase
    {
        public GiosRestApiDataService(IHttpClient httpClient) :
            base(httpClient, @"http://powietrze.gios.gov.pl/pjp/rest/")
        {
        }

        public override Guid Id { get; } = new Guid("DE31CB91-5FAE-4C2A-A1D9-70AA3DAA0190");

        public override string Name => "GIOS REST API";

        public override Task<List<Measurement>> GetMeasurementsAsync(Model.Station station, IEnumerable<Parameter> parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<List<Parameter>> GetParametersAsync(Model.Station station, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<Station> GetStationAsync(int id, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public override async Task<List<Station>> GetStationsAsync(CancellationToken cancellationToken)
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
                    int? id = item["id"].Value<int?>();
                    if (!id.HasValue) continue;

                    lista.Add(new Station(id.Value)
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
                    });
                }
                catch (Exception)
                {
                    throw;
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