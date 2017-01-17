﻿using System;
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

        List<Model.Measurement> GetMeasurements(Model.Station station, IEnumerable<Model.Parameter> parameters);

        Task<List<Model.Measurement>> GetMeasurementsAsync(Model.Station station, IEnumerable<Model.Parameter> parameters);

        Task<List<Model.Measurement>> GetMeasurementsAsync(Model.Station station, IEnumerable<Model.Parameter> parameters, CancellationToken cancellationToken);

        List<Model.Parameter> GetParameters(Model.Station station);

        Task<List<Model.Parameter>> GetParametersAsync(Model.Station station);

        Task<List<Model.Parameter>> GetParametersAsync(Model.Station station, CancellationToken cancellationToken);

        List<Model.Station> GetStations();

        Task<List<Model.Station>> GetStationsAsync();

        Task<List<Model.Station>> GetStationsAsync(CancellationToken cancellationToken);

        //Model.StationState GetStationInfo(int stationId);
        //List<Model.StationState> GetStationInfo(IEnumerable<int> stationIds);
        //List<Model.StationState> GetStationInfoAll();
    }
}