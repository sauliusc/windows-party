using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesonet.WindowsParty.Tests
{
    public class TestInvoker : IInvoker
    {
        public void InvokeIfRequired(Action action)
        {
            action.Invoke();
        }
    }
}
