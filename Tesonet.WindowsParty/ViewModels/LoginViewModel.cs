using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Tesonet.WindowsParty.Events;

namespace Tesonet.WindowsParty.ViewModels
{
    public class LoginViewModel : Screen
    {
        #region fields
        readonly IAuthentificationService _authentificationService;
        string _password;
        bool _setUserNameFocus;
        IEventAggregator _eventAggregator;
        bool _canChangeLogin;
        CancellationTokenSource _cancellationTokenSource;
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

        public bool SetUserNameFocus
        {
            get { return _setUserNameFocus; }
            set
            {
                if (value != _setUserNameFocus)
                    Set(ref _setUserNameFocus, value);
                else
                    NotifyOfPropertyChange("SetUserNameFocus");
            }
        }
        

        public bool CanChangeLogin
        {
            get { return _canChangeLogin; }
            set
            {
                Set(ref _canChangeLogin, value);
                NotifyOfPropertyChange("LoginActionText");
            }
        }

        public string LoginActionText
        {
            get { return CanChangeLogin ? "Log in" : "Cancel"; }
        }
        #endregion properties

        #region Contstructors
        public LoginViewModel(IAuthentificationService authentificationService, IEventAggregator eventAggregator)
        {
            _authentificationService = authentificationService;
            _eventAggregator = eventAggregator;
            CanChangeLogin = true;
        }
        #endregion

        #region Public methods
        public async Task Login(string userName, string password)
        {
            try
            {
                if (!CanChangeLogin)
                {
                    _cancellationTokenSource.Cancel();
                }
                else
                {
                    _cancellationTokenSource = new CancellationTokenSource();
                    CanChangeLogin = false;
                    var loginSuccess = await _authentificationService.Login(userName, password, _cancellationTokenSource.Token);
                    if (loginSuccess)
                        _eventAggregator.PublishOnUIThread(new UserActionEvent(UserAction.Login));
                }
            } catch (TaskCanceledException)
            {
                LogManager.GetLog(this.GetType()).Info("Login action cancelled by user");
            }
            finally
            {
                _cancellationTokenSource = null;
                CanChangeLogin = true;
            }

        }

        public void ClearInput()
        {
            Password = string.Empty;
            CanChangeLogin = true;
        }

        public bool CanLogin(string userName, string password) => 
            !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password);

        protected override void OnActivate()
        {
            base.OnActivate();
            SetUserNameFocus = false;
            SetUserNameFocus = true;
        }
    }
        #endregion
}
