using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Sandbox.HitMe.Portal.Domain.Models;

namespace Sandbox.HitMe.Portal.Domain
{
    public class RemoveClientService :
        IRemoveClientService
    {
        readonly ConcurrentDictionary<string, ClientModel> _clientsCache;
        readonly IHubConnectionContext<dynamic> _clients;

        public RemoveClientService(
            ConcurrentDictionary<string, ClientModel> clientsCache,
            IHubConnectionContext<dynamic> clients)
        {
            _clientsCache = clientsCache;
            _clients = clients;
        }

        public async Task ExecuteAsync(string clientId)
        {
            ClientModel client;
            if (_clientsCache.TryRemove(clientId, out client))
            {
                _clients.All.Remove(
                    new
                    {
                        id = client.Id
                    });
            }
        }
    }
}