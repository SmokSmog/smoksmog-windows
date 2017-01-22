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

        public List<Measurement> GetMeasurements(Model.Station station, IEnumerable<Parameter> parameters)
            => GetMeasurementsAsync(station, parameters).Result;

        public Task<List<Measurement>> GetMeasurementsAsync(Model.Station station, IEnumerable<Parameter> parameters)
            => GetMeasurementsAsync(station, parameters, new CancellationToken());

        public abstract Task<List<Measurement>> GetMeasurementsAsync(Model.Station station, IEnumerable<Parameter> parameters, CancellationToken cancellationToken);

        public List<Parameter> GetParameters(Model.Station station)
        {
            try
            {
                List<Parameter> result = null;
                using (var A = AsyncHelper.Wait)
                {
                    A.Run(GetParametersAsync(station), res => result = res);
                }
                return result;
            }
            catch (Exception ex)
            {
                Diagnostics.Logger.Log(ex);
                throw ex.InnerException;
            }
        }

        public Task<List<Parameter>> GetParametersAsync(Model.Station station)
            => GetParametersAsync(station, new CancellationToken());

        public abstract Task<List<Parameter>> GetParametersAsync(Model.Station station, CancellationToken cancellationToken);

        public Station GetStation(int id)
        {
            try
            {
                Station result = null;
                using (var A = AsyncHelper.Wait)
                {
                    A.Run(GetStationAsync(id), res => result = res);
                }
                return result;
            }
            catch (Exception ex)
            {
                Diagnostics.Logger.Log(ex);
                throw ex.InnerException;
            }
        }

        public Task<Station> GetStationAsync(int id)
            => GetStationAsync(id, new CancellationToken());

        public abstract Task<Station> GetStationAsync(int id, CancellationToken token);

        public List<Station> GetStations()
        {
            try
            {
                List<Station> result = null;
                using (var A = AsyncHelper.Wait)
                {
                    A.Run(GetStationsAsync(), res => result = res);
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