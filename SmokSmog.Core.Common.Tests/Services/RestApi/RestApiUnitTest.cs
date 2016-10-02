﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmokSmog.Services.RestApi;
using SmokSmog.Services.Storage;

namespace SmokSmog.Core.Common.Tests.Services.RestApi
{
    [TestClass]
    public class RestApiUnitTest
    {
        private SmokSmogApiDataService _service;

        private Mock<ISettingsService> _settingsMock;

        [TestInitialize]
        public void Initialize()
        {
            _settingsMock = new Mock<ISettingsService>(MockBehavior.Default);
            _settingsMock.Setup(x => x.Language).Returns("Polish");
            _settingsMock.Setup(x => x.LanguageCode).Returns("pl");

            _service = new SmokSmogApiDataService(_settingsMock.Object);
        }

        [TestMethod]
        public void GetStationsTest()
        {
            var stations = _service.GetStations();
            Assert.IsNotNull(stations);
            var list = stations.ToList();
            Assert.IsTrue(list.Count > 0);
        }
    }
}