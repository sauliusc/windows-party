using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesonet.WindowsParty.Events;
using Tesonet.WindowsParty.Model;

namespace Tesonet.WindowsParty.ViewModels
{
    public class ServerListViewModel : Screen
    {
        #region fields
        IEventAggregator _eventAggregator;
        IDataService _dataService;
        bool _isDataLoading;
        Server _selectedServer;
        #endregion

        #region Properties 
        public ObservableCollection<Server> Servers { get; private set; }
        public bool IsDataLoading
        {
            get { return _isDataLoading; }
            set
            {
                Set(ref _isDataLoading, value);
            }
        }

        public Server SelectedServer
        {
            get { return _selectedServer; }
            set
            {
                Set(ref _selectedServer, value);
            }
        }
        #endregion

        #region Constructors
        public ServerListViewModel(IEventAggregator eventAggregator, IDataService dataService)
        {
            _eventAggregator = eventAggregator;
            _dataService = dataService;
            Servers = new ObservableCollection<Server>();
            _dataService.SetRefreshServerListCompleteAction(RefreshServerListCompleted);
        }
        #endregion

        #region Public methods
        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            RefreshSrverList();
        }

        public void Logout()
        {
            _eventAggregator.PublishOnUIThread(new UserActionEvent(UserAction.Logout));
        }

        public void RefreshSrverList()
        {
            IsDataLoading = true;
            _dataService.RefreshServerList();
        }

        public void RefreshServerListCompleted(IEnumerable<Server> servers)
        {
            IoC.Get<IInvoker>().InvokeIfRequired(
                () =>
                {
                    Servers.Clear();
                    if (servers != null)
                    {
                        foreach (var server in servers)
                            Servers.Add(server);
                    }
                    IsDataLoading = false;
                });
        }
        #endregion
    }
}
