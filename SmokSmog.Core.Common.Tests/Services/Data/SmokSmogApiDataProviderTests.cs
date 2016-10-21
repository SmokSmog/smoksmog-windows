using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmokSmog.Services.Data;
using SmokSmog.Services.Storage;

namespace SmokSmog.Core.Common.Tests.Services.Data
{
    [TestClass]
    public class SmokSmogApiDataProviderTests
    {
        private IDataProvider _service;

        private Mock<ISettingsService> _settingsMock;

        public SmokSmogApiDataProviderTests()
        {
            _settingsMock = new Mock<ISettingsService>(MockBehavior.Default);
            _settingsMock.Setup(x => x.Language).Returns("Polish");
            _settingsMock.Setup(x => x.LanguageCode).Returns("pl-PL");

            _service = new SmokSmogApiDataProvider(_settingsMock.Object);
        }

        [TestMethod]
        public void GetStations_Success()
        {
            var stations = _service.GetStations();
            Assert.IsNotNull(stations);
            Assert.IsTrue(stations.Count() > 0);
        }

        [TestMethod]
        public void GetParticulates_Success()
        {
            // stationId = 4 - Kraków - Aleja Krasińskiego
            var particulates = _service.GetParticulates(4);
            Assert.IsNotNull(particulates);
            Assert.IsTrue(particulates.Count() > 0);
        }
    }
}