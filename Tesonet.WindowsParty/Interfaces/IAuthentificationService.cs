using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesonet.WindowsParty
{
    public interface IAuthentificationService
    {
        void SetActionAfterLogin(Action actionAfterLogin);
        void Login(string username, string password);
        void Logout();
        bool IsUserLoggedIn { get; }
        string SecurityToken { get; }
    }
}
