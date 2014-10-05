using System;
using System.Threading.Tasks;
using Antix.Logging;
using Microsoft.AspNet.SignalR;
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
        readonly IClientStatusService _clientStatusService;

        public ClientsHub(
            Log.Delegate log,
            IGeoLocationService geoLocationService,
            IAddClientService addClientService,
            IRemoveClientService removeClientService,
            IClientStatusService clientStatusService)
        {
            _log = log;
            _geoLocationService = geoLocationService;
            _addClientService = addClientService;
            _removeClientService = removeClientService;
            _clientStatusService = clientStatusService;
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
                    Location = await GetLocation(ipAddress)
                }
            };

            await _addClientService.ExecuteAsync(client);

            if (client.IP.Location == LocationModel.Origin)
            {
                await _clientStatusService.ExecuteAsync(new ClientStatusModel
                {
                    Id = Context.ConnectionId,
                    Type = ClientStatusType.Error,
                    Message = ErrorCodes.ERROR_LOCATION_SERVICE
                });
            }

            await base.OnConnected();
        }

        async Task<LocationModel> GetLocation(string ipAddress)
        {
            try
            {
                return await _geoLocationService.ExecuteAsync(ipAddress);
            }
            catch (Exception ex)
            {
                _log.Error(m => m(ex, ErrorCodes.ERROR_LOCATION_SERVICE));

                return LocationModel.Origin;
            }
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