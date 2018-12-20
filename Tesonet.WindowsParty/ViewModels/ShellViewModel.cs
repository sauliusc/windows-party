using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Tesonet.WindowsParty.Events;

namespace Tesonet.WindowsParty.ViewModels
{
    public class ShellViewModel : Conductor<Screen>.Collection.OneActive, IHandle<UserActionEvent>
    {
        #region Fields
        IEventAggregator _eventAggregator;
        LoginViewModel _loginViewModel;
        ServerListViewModel _serverModel;
        #endregion

        #region Constructors
        public ShellViewModel(IEventAggregator eventAggregator, LoginViewModel loginViewModel, ServerListViewModel serverModel)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _loginViewModel = loginViewModel;
            _serverModel = serverModel;
        }
        #endregion

        #region Public methods
        public void Handle(UserActionEvent message)
        {
            switch (message.UserAction)
            {
                case UserAction.Login:
                    ActivateItem(_serverModel);
                    break;
                case UserAction.Logout:
                    IoC.Get<IAuthentificationService>().Logout();
                    ActivateItem(_loginViewModel);
                    _loginViewModel.ClearInput();
                    break;
                default:
                    break;
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            ActivateItem(_loginViewModel);
        }
        #endregion
    }
}
