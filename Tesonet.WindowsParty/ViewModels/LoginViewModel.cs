using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tesonet.WindowsParty.ViewModels
{
    public class LoginViewModel : Screen
    {
        #region fields
        readonly IAuthentificationService _authentificationService;
        string _password;
        #endregion

        #region properties
        public string Password
        {
            get { return _password; }
            set
            {
                Set(ref _password, value);
            }
        }
        #endregion properties

        #region Contstructors
        public LoginViewModel(IAuthentificationService authentificationService)
        {
            _authentificationService = authentificationService;
        }
        #endregion

        #region Public methods
        public void Login(string userName, string password)
        {
            _authentificationService.Login(userName, password);
        }

        public bool CanLogin(string userName, string password) => 
            !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password);
        #endregion
    }
}
