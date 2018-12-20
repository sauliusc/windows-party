using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tesonet.WindowsParty
{
    public interface IAuthentificationService
    {
        Task<bool> Login(string username, string password, CancellationToken cancellationToken);
        void Logout();
        bool IsUserLoggedIn { get; }
        string SecurityToken { get; }
    }
}
