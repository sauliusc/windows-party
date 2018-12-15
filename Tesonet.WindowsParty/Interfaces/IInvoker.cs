using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesonet.WindowsParty
{
    public interface IInvoker
    {
        void InvokeIfRequired(Action action);
    }
}
