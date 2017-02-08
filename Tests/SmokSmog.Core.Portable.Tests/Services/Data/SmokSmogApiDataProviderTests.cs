using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SmokSmog.Net.Http;
using SmokSmog.Services.Data;
using SmokSmog.Services.Storage;
using System;
using System.Linq;
using System.Threading;

namespace SmokSmog.Core.Portable.Tests.Services.Data
{
    [TestClass]
    public class SmokSmogApiDataProviderTests
    {
        private static IHttpClient _httpClientOffline;
        private static IHttpClient _httpClientOnline;

        private static IDataProvider _serviceOffline;
        private static IDataProvider _serviceOnline;

        private static Mock<IStorageService> _settingsMock;

        [ClassCleanup]
        public static void ClassCleanup()
        {
            //_serviceOnline = null;
            //_serviceOffline = null;
            //_settingsMock = null;
            //_httpClientOnline.Dispose();
            //_httpClientOffline.Dispose();
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _settingsMock = new Mock<IStorageService>(MockBehavior.Default);
            _settingsMock.Setup(x => x.Language).Returns("Polish");
            _settingsMock.Setup(x => x.LanguageCode).Returns("pl-PL");

            var httpClientOfflineMock = new Mock<IHttpClient>();

            // handler.Setup(x => x.GetAsync(It.IsAny<Uri>())).Returns( () => { throw new
            // HttpRequestException(); });

            //public Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken);

            httpClientOfflineMock.Setup(x => x.GetAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Net.Http.HttpRequestException());
            httpClientOfflineMock.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Net.Http.HttpRequestException());

            // .ThrowsAsync(new HttpRequestException());

            //var handler = new Mock<HttpMessageHandler>();
            //handler.Protected()
            //    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            //    .Returns(Task<HttpResponseMessage>.Factory.StartNew(() =>
            //    {
            //        throw new HttpRequestException();
            //        //return new HttpResponseMessage(HttpStatusCode.OK);
            //    }))
            //    .Callback<HttpRequestMessage, CancellationToken>((r, c) =>
            //    {
            //        //Assert.AreEqual(HttpMethod.Get, r.Method);
            //    });

            _httpClientOnline = new HttpClientProxy(new HttpClient());
            _httpClientOffline = httpClientOfflineMock.Object;

            _serviceOnline = new SmokSmogApiDataProvider(_httpClientOnline, _settingsMock.Object);
            _serviceOffline = new SmokSmogApiDataProvider(_httpClientOffline, _settingsMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public void GetParameters_Offline_Throw_HttpRequestException()
        {
            // stationId = 4 - Kraków - Aleja Krasińskiego
            var parameters = _serviceOffline.GetParameters(new Model.Station(4));
        }

        [TestMethod]
        public void GetParameters_Online_Success()
        {
            // stationId = 4 - Kraków - Aleja Krasińskiego
            var parameters = _serviceOnline.GetParameters(new Model.Station(4));
            Assert.IsNotNull(parameters);
            Assert.IsTrue(parameters.Any());
        }

        [TestMethod]
        [ExpectedException(typeof(Net.Http.HttpRequestException))]
        public void GetStations_Offline_Throw_HttpRequestException()
        {
            var stations = _serviceOffline.GetStations();
        }

        [TestMethod]
        public void GetStations_Online_Success()
        {
            var stations = _serviceOnline.GetStations();
            Assert.IsNotNull(stations);
            Assert.IsTrue(stations.Any());
        }
    }
}