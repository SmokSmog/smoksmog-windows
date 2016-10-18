using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmokSmog.Model;

namespace SmokSmog.Services.Data
{
    public abstract class AsyncDataProviderBase : IDataProvider
    {
        public abstract Guid Id { get; }

        public abstract string Name { get; }

        public IEnumerable<Measurement> GetMeasurements(int stationId)
            => GetMeasurementsAsync(stationId).Result;

        public Task<IEnumerable<Measurement>> GetMeasurementsAsync(int stationId)
            => GetMeasurementsAsync(stationId, new CancellationToken());

        public abstract Task<IEnumerable<Measurement>> GetMeasurementsAsync(int stationId, CancellationToken cancellationToken);

        public IEnumerable<Parameter> GetParticulates(int stationId)
            => GetParticulatesAsync(stationId).Result;

        public Task<IEnumerable<Parameter>> GetParticulatesAsync(int stationId)
            => GetParticulatesAsync(stationId, new CancellationToken());

        public abstract Task<IEnumerable<Parameter>> GetParticulatesAsync(int stationId, CancellationToken cancellationToken);

        public IEnumerable<Station> GetStations() => GetStationsAsync().Result;

        public Task<IEnumerable<Station>> GetStationsAsync() => GetStationsAsync(new CancellationToken());

        public abstract Task<IEnumerable<Station>> GetStationsAsync(CancellationToken cancellationToken);
    }
}