using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesonet.WindowsParty.Interfaces;
using Tesonet.WindowsParty.Model;
using Flurl;
using Flurl.Http;
using System.Threading;

namespace Tesonet.WindowsParty.Services
{
    public class DataService : IDataService
    {
        #region fields
        IAuthentificationService _authentificationService;
        IConfigurationService _configurationService;
        IInvoker _invoker;
        #endregion

        #region constructors
        public DataService(IAuthentificationService authentificationService, IConfigurationService configurationService, IInvoker invoker)
        {
            _authentificationService = authentificationService;
            _configurationService = configurationService;
            _invoker = invoker;
        }
        #endregion

        #region public methods
        public async Task<IEnumerable<Server>> GetServerList(CancellationToken cancellationToken)
        {
            try
            {
                var serverList = await _configurationService.BaseServiceUrl.WithOAuthBearerToken(_authentificationService.SecurityToken)
                                                            .AppendPathSegment(_configurationService.ServerListAction).GetJsonAsync<IEnumerable<Server>>(cancellationToken);
#if DEBUG
                await Task.Delay(1000, cancellationToken);
#endif
                return serverList;
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _invoker.InvokeIfRequired(() => throw ex);
                return null;
            }
        }
#endregion

#region private methods
#endregion
    }
}
