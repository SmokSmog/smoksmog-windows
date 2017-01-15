using SmokSmog.Model;
using SmokSmog.Threading;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmokSmog.Services.Data
{
    public abstract class AsyncDataProviderBase : IDataProvider
    {
        public abstract Guid Id { get; }

        public abstract string Name { get; }

        public List<Measurement> GetMeasurements(int stationId, IEnumerable<Parameter> parameters)
            => GetMeasurementsAsync(stationId, parameters).Result;

        public Task<List<Measurement>> GetMeasurementsAsync(int stationId, IEnumerable<Parameter> parameters)
            => GetMeasurementsAsync(stationId, parameters, new CancellationToken());

        public abstract Task<List<Measurement>> GetMeasurementsAsync(int stationId, IEnumerable<Parameter> parameters, CancellationToken cancellationToken);

        public List<Parameter> GetParameters(int stationId)
        {
            try
            {
                List<Parameter> result = null;
                using (var A = AsyncHelper.Wait)
                {
                    A.Run<List<Parameter>>(GetParametersAsync(stationId), res => result = res);
                }
                return result;
            }
            catch (Exception ex)
            {
                Diagnostics.Logger.Log(ex);
                throw ex.InnerException;
            }
        }

        public Task<List<Parameter>> GetParametersAsync(int stationId)
            => GetParametersAsync(stationId, new CancellationToken());

        public abstract Task<List<Parameter>> GetParametersAsync(int stationId, CancellationToken cancellationToken);

        public List<Station> GetStations()
        {
            try
            {
                List<Station> result = null;
                using (var A = AsyncHelper.Wait)
                {
                    A.Run<List<Station>>(GetStationsAsync(), res => result = res);
                }
                return result;
            }
            catch (Exception ex)
            {
                Diagnostics.Logger.Log(ex);
                throw ex.InnerException;
            }
        }

        public Task<List<Station>> GetStationsAsync() => GetStationsAsync(new CancellationToken());

        public abstract Task<List<Station>> GetStationsAsync(CancellationToken cancellationToken);
    }
}