using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Sandbox.HitMe.Portal.Domain.Models;

namespace Sandbox.HitMe.Portal.Domain
{
    public class HitClientService :
        IHitClientService
    {
        readonly IHubConnectionContext<dynamic> _clients;

        public HitClientService(IHubConnectionContext<dynamic> clients)
        {
            _clients = clients;
        }

        public async Task ExecuteAsync(HitModel model)
        {
            _clients.All.Hit(new
            {
                fromIP = new
                {
                    address = model.FromIP.Address,
                    location = new
                    {
                        longitude = model.FromIP.Location.Longitude,
                        latitude = model.FromIP.Location.Latitude
                    }
                },
                toClientId = model.ToClientId
            });
        }
    }
}