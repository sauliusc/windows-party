using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tesonet.WindowsParty.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Tesonet.WindowsParty.Tests
{
    [TestClass]
    public class TesonetServiceTest
    {
        private const string _testUser = "tesonet";
        private const string _testPassword = "partyanimal";
        TestConfigurationService _configuration;
        IAuthentificationService _authentification;
        CancellationTokenSource _cancellationTokenSource;
        IInvoker _invoker;  

        [TestInitialize]
        public void Setup()
        {
            _invoker = new TestInvoker();
            _configuration = new TestConfigurationService();
            _configuration.Setup("http://playground.tesonet.lt/v1/", "tokens", "servers");
            _authentification = new AuthentificationService(_configuration, _invoker);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        [TestMethod]
        public async Task TestLoginSuccess()
        {
            var result = await _authentification.Login(_testUser, _testPassword, _cancellationTokenSource.Token);
            Assert.IsTrue(_authentification.IsUserLoggedIn, "AuthentificationService failed authorize user");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "AuthentificationService approved wrong user")]
        public async Task TestLoginFailed()
        {
            var result = await _authentification.Login(_testUser, "partya", _cancellationTokenSource.Token);
        }

        [TestMethod]
        public async Task TestServerListGet()
        {
            await _authentification.Login(_testUser, _testPassword, _cancellationTokenSource.Token);
            IDataService dataService = new DataService(_authentification, _configuration, _invoker);
            var serverList = await dataService.GetServerList(_cancellationTokenSource.Token);
            Assert.AreNotEqual(0, serverList.Count(), "Server list not returned data");
            }
    }
}
