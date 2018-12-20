using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesonet.WindowsParty.Interfaces;

namespace Tesonet.WindowsParty.Tests
{
    class TestConfigurationService : IConfigurationService
    {
        public string BaseServiceUrl { get; private set; }

        public string AuthentificationAction { get; private set; }

        public string ServerListAction { get; private set; }

        public string TraceLogFile => throw new NotImplementedException();

        public string ErrorLogFile => throw new NotImplementedException();

        public void Setup(string service, string authentificationAction, string serverListAction)
        {
            BaseServiceUrl = service;
            AuthentificationAction = authentificationAction;
            ServerListAction = serverListAction;
        }
    }
}
