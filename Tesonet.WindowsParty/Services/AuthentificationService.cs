using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesonet.WindowsParty.Interfaces;

namespace Tesonet.WindowsParty.Services
{
    public class AuthentificationService : IAuthentificationService
    {
        #region Fields
        Action _actionAfterLogin;
        IConfigurationService _configurationService;
        RestClient _client;
        bool _executeAsync;
        IInvoker _invoker;
        #endregion

        #region Constructors
        public AuthentificationService(IConfigurationService configurationService, IInvoker invoker, bool executeAsync = true)
        {
            _configurationService = configurationService;
            _client = new RestClient(configurationService.BaseServiceUrl);
            _invoker = invoker;
            _executeAsync = executeAsync;
        }
        #endregion

        #region Properties
        public bool IsUserLoggedIn { get { return !string.IsNullOrEmpty(SecurityToken); } }
        public string SecurityToken { get; private set; }
        #endregion

        #region public methods
        public void Login(string username, string password)
        {
            var request = new RestRequest(_configurationService.AuthentificationAction);
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.POST;
            request.AddBody(new { username, password });
            if (_executeAsync)
            {
                RestRequestAsyncHandle va = _client.ExecuteAsync(request, OnLoginResult);
            } else
            {
                OnLoginResult(_client.Execute(request));
            }
        }

        public void Logout()
        {
            SecurityToken = null;
        }

        public void SetActionAfterLogin(Action actionAfterLogin)
        {
            _actionAfterLogin = actionAfterLogin;
        }
        #endregion

        #region private methods
        private void OnLoginResult(IRestResponse response)
        {
            try
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JObject responseObject = JObject.Parse(response.Content);
                    SecurityToken = (string)responseObject["token"];
                    _actionAfterLogin.Invoke();
                }
                else
                {
                    throw new Exception("Login failed");
                }
            }catch (Exception ex)
            {
                _invoker.InvokeIfRequired(
                () =>
                {
                    throw ex;
                });
            }
        }
        #endregion
    }
}
