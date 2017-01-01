using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmokSmog.Services;

namespace SmokSmog.Core.Desktop.Tests.Services
{
    [TestClass]
    public class ServiceLocatorUnitTest
    {
        IServiceLocator _serviceLocator;

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
