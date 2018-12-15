using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesonet.WindowsParty.Model;

namespace Tesonet.WindowsParty
{
    public interface IDataService
    {
        void RefreshServerList();
        void SetRefreshServerListCompleteAction(Action<IEnumerable<Server>> action);
    }
}
