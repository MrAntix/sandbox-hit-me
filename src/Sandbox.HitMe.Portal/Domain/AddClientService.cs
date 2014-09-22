using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Sandbox.HitMe.Portal.Domain.Models;

namespace Sandbox.HitMe.Portal.Domain
{
    public class AddClientService :
        IAddClientService
    {
        readonly ConcurrentDictionary<string, ClientModel> _clientsCache;
        readonly IHubConnectionContext<dynamic> _clients;

        public AddClientService(
            ConcurrentDictionary<string, ClientModel> clientsCache,
            IHubConnectionContext<dynamic> clients)
        {
            _clientsCache = clientsCache;
            _clients = clients;
        }

        public async Task ExecuteAsync(ClientModel client)
        {
            foreach (var other in _clientsCache.Values)
            {
                _clients.Client(client.Id).Add(Map(other));
            }

            _clientsCache.AddOrUpdate(client.Id, client, (s, existing) => client);

            _clients.All.Add(Map(client));
        }

        static object Map(ClientModel client)
        {
            return new
            {
                id = client.Id,
               // name = client.IP.Address,
                location = new
                {
                    latitude = client.IP.Location.Latitude,
                    longitude = client.IP.Location.Longitude
                }
            };
        }
    }
}