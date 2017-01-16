using SmokSmog.Model;
using SmokSmog.Services.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmokSmog.Design.Services
{
    public class DesignDataProvider : AsyncDataProviderBase
    {
        public override Guid Id { get; } = new Guid("{5FC2BA10-542B-4D61-A328-C5E78BED0E09}");

        public override string Name => "Design Time Data Provider";

        public override Task<List<Measurement>> GetMeasurementsAsync(Model.Station station, IEnumerable<Parameter> parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<List<Parameter>> GetParametersAsync(Model.Station station, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

#pragma warning disable 1998

        public override async Task<List<Station>> GetStationsAsync(CancellationToken cancellationToken)
        {
            var result = new List<Station>()
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

            return result;
        }
    }
}