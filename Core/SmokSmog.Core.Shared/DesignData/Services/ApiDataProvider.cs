using SmokSmog.Model;
using SmokSmog.Services.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmokSmog.DesignData.Services
{
    public class ApiDataProvider : AsyncDataProviderBase
    {
        public override Guid Id { get; } = new Guid("{5FC2BA10-542B-4D61-A328-C5E78BED0E09}");

        public override string Name => "Design Time Data Provider";

#pragma warning disable 1998

        public override async Task<List<Measurement>> GetMeasurementsAsync(Model.Station station, IEnumerable<Parameter> parameters, CancellationToken cancellationToken)
        {
            return new List<Measurement>()
            {
                new Measurement( station, new Parameter(station, 7)) {  Date= DateTime.Parse("01.01.2017 18:00:00"), Avg1Hour= 81.3 , Avg24Hour =   75.2 },
                new Measurement( station, new Parameter(station, 1)) {  Date= DateTime.Parse("01.01.2017 18:00:00"), Avg1Hour= 16.2 , Avg24Hour =   16.8 },
                new Measurement( station, new Parameter(station, 3)) {  Date= DateTime.Parse("01.01.2017 18:00:00"), Avg1Hour= 37.15, Avg24Hour =   45.3 },
                new Measurement( station, new Parameter(station, 4)) {  Date= DateTime.Parse("01.01.2017 18:00:00"), Avg1Hour=610.94, Avg24Hour = 1564.2 },
                new Measurement( station, new Parameter(station, 5)) {  Date= DateTime.Parse("01.01.2017 18:00:00"), Avg1Hour= 28.92, Avg24Hour =   30.3 },
                new Measurement( station, new Parameter(station,11)) {  Date= DateTime.Parse("01.01.2017 18:00:00"), Avg1Hour=  5.66, Avg24Hour =    8.1 },
            };
        }

#pragma warning disable 1998

        public override async Task<List<Parameter>> GetParametersAsync(Model.Station station, CancellationToken cancellationToken)
        {
            return new List<Parameter>()
            {
                new Parameter(station, 7){ Name="Pył zawieszony",    ShortName="PM₁₀",   Unit="µg/m³", Norm = new Norm() { Aggregation = AggregationType.Avg24Hour, Name = "WIOŚ", Value = 50} },
                new Parameter(station, 1){ Name="Dwutlenek siarki",  ShortName="SO₂",    Unit="µg/m³", Norm = new Norm() { Aggregation = AggregationType.Avg24Hour, Name = "WIOŚ", Value =350} },
                new Parameter(station, 3){ Name="Dwutlenek azotu",   ShortName="NO₂",    Unit="µg/m³", Norm = new Norm() { Aggregation = AggregationType.Avg24Hour, Name = "WIOŚ", Value =200} },
                new Parameter(station, 4){ Name="Tlenek węgla",      ShortName="CO",     Unit="µg/m³", Norm = new Norm() { Aggregation = AggregationType.Avg24Hour, Name = "WIOŚ", Value =10000} },
                new Parameter(station, 5){ Name="Ozon",              ShortName="O₃",     Unit="µg/m³", Norm = new Norm() { Aggregation = AggregationType.Avg24Hour, Name = "WIOŚ", Value =120} },
                new Parameter(station, 11){ Name="Benzen",           ShortName="C₆H₆",   Unit="µg/m³", Norm = new Norm() { Aggregation = AggregationType.Avg1Year, Name = "WIOŚ", Value =5} },
            };
        }

#pragma warning disable 1998

        public override async Task<Station> GetStationAsync(int id, CancellationToken token)
        {
            return Station.Sample;
        }

#pragma warning disable 1998

        public override async Task<List<Station>> GetStationsAsync(CancellationToken cancellationToken)
        {
            var result = new List<Station>()
            {
                new Station(1) { Name="Andrychów", City="Andrychów", Province="Małopolska" },
                new Station(2) { Name="Kraków - Ditla", City="Kraków", Province="Małopolska" },
                new Station(3) { Name="Kraków - Bronowice", City="Kraków", Province="Małopolska" },
                new Station(4) { Name="Kraków - Aleja Kraśińskiego", City="Kraków", Province="Małopolska" },
                new Station(5) { Name="Kraków - Nowa Huta", City="Kraków", Province="Małopolska" },
                new Station(6) { Name="Kraków - Bierzanów", City="Kraków", Province="Małopolska" },
                new Station(7) { Name="Tarnów", City="Tarnów", Province="Małopolska"},
                new Station(8) { Name="Nowy Sącz", City="Nowy Sącz", Province="Małopolska"},
            };

            return result;
        }
    }
}