using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SmokSmog.Services;

namespace SmokSmog.Core.WinRT.Tests.Services.Services
{
    [TestClass]
    public class ServiceLocatorUnitTest
    {
        private IServiceLocator _serviceLocator;

        [TestInitialize]
        public void Initialize()
        {
            _serviceLocator = new ServiceLocator();
        }

        [TestMethod]
        public void GetDataService()
        {
            Assert.IsNotNull(_serviceLocator.DataService);
        }
    }
}