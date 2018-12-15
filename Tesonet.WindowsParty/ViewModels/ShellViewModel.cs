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
    public class ShellViewModel : Screen, IHandle<UserActionEvent>
    {
        #region Fields
        IEventAggregator _eventAggregator;
        INavigationService _navigationService;
        #endregion

        #region Constructors
        public ShellViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            IoC.Get<IAuthentificationService>().SetActionAfterLogin(() => _eventAggregator.PublishOnUIThread(new UserActionEvent(UserAction.Login)));
            _eventAggregator.Subscribe(this);
        }
        #endregion

        #region Public methods
        public void Handle(UserActionEvent message)
        {
            switch (message.UserAction)
            {
                case UserAction.Login:
                    _navigationService.NavigateToViewModel<ServerListViewModel>();
                    break;
                case UserAction.Logout:
                    IoC.Get<IAuthentificationService>().Logout();
                    _navigationService.GoBack();
                    break;
                default:
                    break;
            }
        }

        public void RegisterFrame(Frame frame)
        {
            _navigationService = new FrameAdapter(frame);
            _navigationService.NavigateToViewModel<LoginViewModel>();
        }
        #endregion
    }
}
