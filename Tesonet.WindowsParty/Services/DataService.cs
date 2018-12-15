using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesonet.WindowsParty.Interfaces;
using Tesonet.WindowsParty.Model;

namespace Tesonet.WindowsParty.Services
{
    public class DataService : IDataService
    {
        #region fields
        IAuthentificationService _authentificationService;
        IConfigurationService _configurationService;
        Action<IEnumerable<Server>> _refreshServerListCompleteAction;
        bool _executeAsync;
        RestClient _client;
        #endregion

        #region constructors
        public DataService(IAuthentificationService authentificationService, IConfigurationService configurationService, bool executeAsync = true)
        {
            _authentificationService = authentificationService;
            _configurationService = configurationService;
            _client = new RestClient(configurationService.BaseServiceUrl);
            _executeAsync = executeAsync;
        }
        #endregion

        #region public methods
        public void RefreshServerList()
        {
            var request = new RestRequest(_configurationService.ServerListAction);
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.GET;
            request.AddHeader("Authorization", $"Bearer {_authentificationService.SecurityToken}");
            if (_executeAsync)
            {
                _client.ExecuteAsync(request, OnGetServerList);
            } else
            {
                OnGetServerList(_client.Execute(request));
            }
        }
        public void SetRefreshServerListCompleteAction(Action<IEnumerable<Server>> action)
        {
            _refreshServerListCompleteAction = action;
        }
        #endregion

        #region private methods
        private void OnGetServerList(IRestResponse response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var servers = JsonConvert.DeserializeObject<List<Server>>(response.Content);
                servers.Add(new Server() { Name = "Saulius #1 wpf developer", Distance = 1 });
                _refreshServerListCompleteAction.Invoke(servers);
            }
            else
            {
                _refreshServerListCompleteAction.Invoke(null);
            }
        }
        #endregion
    }
}
