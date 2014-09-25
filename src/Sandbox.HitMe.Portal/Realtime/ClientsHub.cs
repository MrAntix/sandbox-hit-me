using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Antix.Logging;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Sandbox.HitMe.Portal.Domain;
using Sandbox.HitMe.Portal.Domain.Models;

namespace Sandbox.HitMe.Portal.Realtime
{
    public class ClientsHub : Hub
    {
        readonly Log.Delegate _log;
        readonly IGeoLocationService _geoLocationService;
        readonly IAddClientService _addClientService;
        readonly IRemoveClientService _removeClientService;

        public ClientsHub(
            Log.Delegate log,
            IGeoLocationService geoLocationService,
            IAddClientService addClientService,
            IRemoveClientService removeClientService)
        {
            _log = log;
            _geoLocationService = geoLocationService;
            _addClientService = addClientService;
            _removeClientService = removeClientService;
        }

        public override async Task OnConnected()
        {
            _log.Debug(m => m("connected {0}", Context.ConnectionId));

            var ipAddress = GetRemoteClientIPAddress(Context.Request);

            var client = new ClientModel
            {
                Id = Context.ConnectionId,
                IP = new IPModel
                {
                    Address = ipAddress,
                    Location = await _geoLocationService.ExecuteAsync(ipAddress)
                }
            };

            await _addClientService.ExecuteAsync(client);

            await base.OnConnected();
        }

        public override async Task OnReconnected()
        {
            _log.Debug(m => m("reconnected {0}", Context.ConnectionId));

            await base.OnReconnected();
        }

        public override async Task OnDisconnected(bool stopCalled)
        {
            _log.Debug(m => m("disconnected {0}", Context.ConnectionId));

            await _removeClientService.ExecuteAsync(Context.ConnectionId);

            await base.OnDisconnected(stopCalled);
        }

        static string GetRemoteClientIPAddress(
            IRequest request)
        {
            const string key = "server.RemoteIpAddress";
            return request.Environment.ContainsKey(key)
                ? (string) request.Environment[key]
                : null;
        }
    }
}