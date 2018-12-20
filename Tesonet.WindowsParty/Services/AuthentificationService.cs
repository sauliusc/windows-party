using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesonet.WindowsParty.Interfaces;
using Tesonet.WindowsParty.Model;

namespace Tesonet.WindowsParty.Services
{
    public class AuthentificationService : IAuthentificationService
    {
        #region Fields
        IConfigurationService _configurationService;
        IInvoker _invoker;
        #endregion

        #region Constructors
        public AuthentificationService(IConfigurationService configurationService, IInvoker invoker)
        {
            _configurationService = configurationService;
            _invoker = invoker;
        }
        #endregion

        #region Properties
        public bool IsUserLoggedIn { get { return !string.IsNullOrEmpty(SecurityToken); } }
        public string SecurityToken { get; private set; }
        #endregion

        #region public methods
        public async Task<bool> Login(string username, string password, CancellationToken cancellationToken)
        {
            try
            {
                var tokenResponse = await _configurationService.BaseServiceUrl
                .WithHeader("Content-Type", "application/json")
                                                    .AppendPathSegment(_configurationService.AuthentificationAction)
                                                    .PostJsonAsync(new { username, password }, cancellationToken).ReceiveJson<LoginToken>();

                SecurityToken = tokenResponse.Token;
                if (string.IsNullOrEmpty(SecurityToken))
                {
                    throw new Exception("Login failed");
                }
#if DEBUG
                await Task.Delay(1000, cancellationToken);
#endif
                return true;
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (FlurlHttpException)
            {
                //full error loged by flurUrl. raising user friendly exception
                _invoker.InvokeIfRequired(() => throw new Exception("Login failed"));
               return false;
            }
            catch (Exception ex)
            {
                _invoker.InvokeIfRequired(() => throw ex);
                return false;
            }
        }

        public void Logout()
        {
            SecurityToken = null;
        }
        
        #endregion
    }
}
