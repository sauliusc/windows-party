using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        CancellationTokenSource _cancellationTokenSource;
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
        }
        #endregion

        #region Public methods

        protected override void OnActivate()
        {
            RefreshServerList();
            base.OnActivate();
        }

        public void Logout()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }
            _eventAggregator.PublishOnUIThread(new UserActionEvent(UserAction.Logout));
        }

        public async void RefreshServerList()
        {
            IsDataLoading = true;
            _cancellationTokenSource = new CancellationTokenSource();
            try
            {
                var loadedServers = await _dataService.GetServerList(_cancellationTokenSource.Token);
                Servers.Clear();
                if (loadedServers != null)
                {
                    foreach (var server in loadedServers)
                        Servers.Add(server);
                }
            } catch (TaskCanceledException)
            {
                LogManager.GetLog(this.GetType()).Info("Server list action cancelled by user");
            }
            finally
            {
                _cancellationTokenSource = null;
                IsDataLoading = false;
            }
        }
        #endregion
    }
}
