using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesonet.WindowsParty.Model;

namespace Tesonet.WindowsParty
{
    public interface IDataService
    {
        Task<IEnumerable<Server>> GetServerList(CancellationToken cancellationToken);
    }
}
