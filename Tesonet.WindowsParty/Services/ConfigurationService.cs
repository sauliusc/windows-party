using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesonet.WindowsParty.Interfaces;

namespace Tesonet.WindowsParty.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public string BaseServiceUrl
        {
            get
            {
                return Properties.Settings.Default.BaseServiceUrl;
            }
        }

        public string AuthentificationAction
        {
            get
            {
                return Properties.Settings.Default.AuthentificationAction;
            }
        }

        public string ServerListAction
        {
            get
            {
                return Properties.Settings.Default.ServerListAction;
            }
        }

        public string TraceLogFile
        {
            get
            {
                return Properties.Settings.Default.TraceLogFile;
            }
        }
        public string ErrorLogFile
        {
            get
            {
                return Properties.Settings.Default.ErrorLogFile;
            }
        }
    }
}
