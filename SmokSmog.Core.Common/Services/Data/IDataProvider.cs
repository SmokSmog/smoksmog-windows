using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmokSmog.Services.Data
{
    public interface IDataProvider
    {
        /// <summary>
        /// Unique Identification number of service
        /// </summary>
        Guid Id { get; }

        string Name { get; }

        IEnumerable<Model.Measurement> GetMeasurements(int stationId);

        Task<IEnumerable<Model.Measurement>> GetMeasurementsAsync(int stationId);

        Task<IEnumerable<Model.Measurement>> GetMeasurementsAsync(int stationId, CancellationToken cancellationToken);

        IEnumerable<Model.Parameter> GetParticulates(int stationId);

        Task<IEnumerable<Model.Parameter>> GetParticulatesAsync(int stationId);

        Task<IEnumerable<Model.Parameter>> GetParticulatesAsync(int stationId, CancellationToken cancellationToken);

        IEnumerable<Model.Station> GetStations();

        Task<IEnumerable<Model.Station>> GetStationsAsync();

        Task<IEnumerable<Model.Station>> GetStationsAsync(CancellationToken cancellationToken);

        //Model.StationState GetStationInfo(int stationId);
        //List<Model.StationState> GetStationInfo(IEnumerable<int> stationIds);
        //List<Model.StationState> GetStationInfoAll();
    }
}