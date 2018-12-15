using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesonet.WindowsParty.Interfaces
{
    public interface IConfigurationService
    {
        string BaseServiceUrl { get; }
        string AuthentificationAction { get; }
        string ServerListAction { get; }
    }
}
