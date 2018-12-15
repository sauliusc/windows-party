using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesonet.WindowsParty.Events
{
    public class UserActionEvent
    {
        public UserActionEvent(UserAction userAction)
        {
            UserAction = userAction;
        }

        public UserAction UserAction { get; private set; }
    }

    public enum UserAction
    {
        Login,
        Logout
    }
}
