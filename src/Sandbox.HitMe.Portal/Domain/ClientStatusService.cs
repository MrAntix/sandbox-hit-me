using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Sandbox.HitMe.Portal.Domain.Models;

namespace Sandbox.HitMe.Portal.Domain
{
    public class ClientStatusService :
        IClientStatusService
    {
        readonly IHubConnectionContext<dynamic> _clients;

        public ClientStatusService(IHubConnectionContext<dynamic> clients)
        {
            _clients = clients;
        }

        public async Task ExecuteAsync(ClientStatusModel model)
        {
            _clients.Client(model.Id).Status(Map(model));
        }

        static object Map(ClientStatusModel model)
        {
            return new
            {
                type = model.Type.ToString("g").ToLower(),
                message = model.Message
            };
        }
    }
}