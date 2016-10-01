using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmokSmog.Services.RestApi
{
    public interface IDataService
    {
        string Name { get; }

        IEnumerable<Model.Measurement> GetStationMeasurements(int stationId);

        Task<IEnumerable<Model.Measurement>> GetStationMeasurementsAsync(int stationId);

        IEnumerable<Model.Parameter> GetStationParticulates(int stationId);

        Task<IEnumerable<Model.Parameter>> GetStationParticulatesAsync(int stationId);

        IEnumerable<Model.Station> GetStations();

        Task<IEnumerable<Model.Station>> GetStationsAsync();

        Model.StationState GetStationInfo(int stationId);

        List<Model.StationState> GetStationInfo(IEnumerable<int> stationIds);

        List<Model.StationState> GetStationInfoAll();
    }
}