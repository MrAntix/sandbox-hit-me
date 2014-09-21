using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace Sandbox.HitMe.Portal.Server
{
    public class ClientsHub : Hub
    {
        static readonly ConcurrentDictionary<string, Client> ClientsCache;

        static ClientsHub()
        {
            ClientsCache = new ConcurrentDictionary<string, Client>();
        }

        public override async Task OnConnected()
        {
            Debug.WriteLine("connected {0}", Context.ConnectionId);

            var ipAddress = (string)Context.Request.Environment["server.RemoteIpAddress"];

            var client = new Client
            {
                Id = Context.ConnectionId,
                IPAddress = ipAddress,
                Location = await GetLocationAsync(ipAddress)
            };

            ClientsCache.AddOrUpdate(client.Id, client, (s, existing) => { return existing; });

            Clients.All.Add(
                new
                {
                    id = client.Id,
                    name = client.IPAddress,
                    location = new
                    {
                        latitude = client.Location.Latitude,
                        longitude = client.Location.Longitude
                    }
                });

            await base.OnConnected();
        }

        async Task<Location> GetLocationAsync(string ipAddress)
        {
            if (ipAddress == null
                || ipAddress.Length < 7) ipAddress = "86.143.154.77";

            var url = string.Format("http://freegeoip.net/json/{0}", ipAddress);
            var webClient = new WebClient();

            var response = await webClient.DownloadStringTaskAsync(url);

            dynamic location = JsonConvert.DeserializeObject(response);

            return new Location
            {
                Latitude = location.latitude,
                Longitude = location.longitude
            };
        }

        public override async Task OnReconnected()
        {
            Debug.WriteLine("reconnected {0}", Context.ConnectionId);

            await base.OnReconnected();
        }

        public override async Task OnDisconnected()
        {
            Debug.WriteLine("disconnected {0}", Context.ConnectionId);

            Client client;
            if (ClientsCache.TryRemove(Context.ConnectionId, out client))
            {
                Clients.All.Remove(
                new
                {
                    id = client.Id
                });
            }

            await base.OnDisconnected();
        }
    }
}