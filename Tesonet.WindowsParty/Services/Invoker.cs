using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Tesonet.WindowsParty.Services
{
    public class Invoker : IInvoker
    {
        public void InvokeIfRequired(Action action)
        {
            if (Application.Current != null && Application.Current.Dispatcher.Thread != Thread.CurrentThread)
            {
                action();
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(action);
            }
        }
    }
}
