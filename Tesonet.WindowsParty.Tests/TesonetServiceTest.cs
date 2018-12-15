using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tesonet.WindowsParty.Services;
using System.Linq;

namespace Tesonet.WindowsParty.Tests
{
    [TestClass]
    public class TesonetServiceTest
    {
        private const string _testUser = "tesonet";
        private const string _testPassword = "partyanimal";
        TestConfigurationService _configuration;
        IAuthentificationService _authentification;

        [TestInitialize]
        public void Setup()
        {
            _configuration = new TestConfigurationService();
            _configuration.Setup("http://playground.tesonet.lt/v1/", "tokens", "servers");
            _authentification = new AuthentificationService(_configuration, new TestInvoker(), false);
        }

        [TestMethod]
        public void TestLoginSuccess()
        {
            _authentification.SetActionAfterLogin(() => Assert.IsTrue(_authentification.IsUserLoggedIn, "AuthentificationService failed authorize user"));
            _authentification.Login(_testUser, _testPassword);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "AuthentificationService approved wrong user")]
        public void TestLoginFailed()
        {
            _authentification.SetActionAfterLogin(() => { return; });
            _authentification.Login(_testUser, "partya");
        }

        [TestMethod]
        public void TestServerListGet()
        {
            _authentification.SetActionAfterLogin(() => { return; });
            _authentification.Login(_testUser, _testPassword);
            IDataService dataService = new DataService(_authentification, _configuration, false);
            dataService.SetRefreshServerListCompleteAction((a) => { Assert.AreEqual(a.Count(), 0, "Server list not returned data"); });
        }
    }
}
