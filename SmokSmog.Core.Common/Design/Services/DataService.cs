using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmokSmog.Model;
using SmokSmog.Services.RestApi;

namespace SmokSmog.Design.Services
{
    public class DataService : IDataService
    {
        public List<StationState> GetStationInfo(IEnumerable<int> stationIds)
        {
            throw new NotImplementedException();
        }

        public StationState GetStationInfo(int stationId)
        {
            throw new NotImplementedException();
        }

        public List<StationState> GetStationInfoAll()
        {
            throw new NotImplementedException();
        }

        public ICollection<Measurement> GetStationMeasurements(int stationId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Measurement>> GetStationMeasurementsAsync(int stationId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Parameter> GetStationParticulates(int stationId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Parameter>> GetStationParticulatesAsync(int stationId)
        {
            throw new NotImplementedException();
        }

        public ICollection<Station> GetStations()
        {
            return new List<Station>()
                {
                    new Station() { Name="Andrychów", City="Andrychów", Province="Małopolska" },
                    new Station() { Name="Kraków - Ditla", City="Kraków", Province="Małopolska" },
                    new Station() { Name="Kraków - Bronowice", City="Kraków", Province="Małopolska" },
                    new Station() { Name="Kraków - Aleja Kraśińskiego", City="Kraków", Province="Małopolska" },
                    new Station() { Name="Kraków - Nowa Huta", City="Kraków", Province="Małopolska" },
                    new Station() { Name="Kraków - Bierzanów", City="Kraków", Province="Małopolska" },
                    new Station() { Name="Tarnów", City="Tarnów", Province="Małopolska"},
                    new Station() { Name="Nowy Sącz", City="Nowy Sącz", Province="Małopolska"},
                    new Station() { Name="Warszawa", City="Warszawa", Province="Mazowieckie" },
                };
        }

#pragma warning disable 1998

        public async Task<ICollection<Station>> GetStationsAsync()
        {
            return GetStations();
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
    }
}