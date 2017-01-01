using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmokSmog.Model;
using SmokSmog.Threading;

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

        public IEnumerable<Parameter> GetParameters(int stationId)
        {
            try
            {
                IEnumerable<Parameter> result = null;
                using (var A = AsyncHelper.Wait)
                {
                    A.Run<IEnumerable<Parameter>>(GetParametersAsync(stationId), res => result = res);
                }
                return result;
            }
            catch (Exception ex)
            {
                Diagnostics.Logger.Log(ex);
                throw ex.InnerException;
            }
        }

        public Task<IEnumerable<Parameter>> GetParametersAsync(int stationId)
            => GetParametersAsync(stationId, new CancellationToken());

        public abstract Task<IEnumerable<Parameter>> GetParametersAsync(int stationId, CancellationToken cancellationToken);

        //public IEnumerable<Station> GetStations() => GetStationsAsync().Result;
        public IEnumerable<Station> GetStations()
        {
            try
            {
                IEnumerable<Station> result = null;
                using (var A = AsyncHelper.Wait)
                {
                    A.Run<IEnumerable<Station>>(GetStationsAsync(), res => result = res);
                }
                return result;
            }
            catch (Exception ex)
            {
                Diagnostics.Logger.Log(ex);
                throw ex.InnerException;
            }
        }

        public Task<IEnumerable<Station>> GetStationsAsync() => GetStationsAsync(new CancellationToken());

        public abstract Task<IEnumerable<Station>> GetStationsAsync(CancellationToken cancellationToken);
    }
}