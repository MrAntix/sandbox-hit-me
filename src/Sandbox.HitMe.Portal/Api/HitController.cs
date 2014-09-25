using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Antix.Http.Filters.Logging;
using Sandbox.HitMe.Portal.Domain;
using Sandbox.HitMe.Portal.Domain.Models;

namespace Sandbox.HitMe.Portal.Api
{
    [LogAction]
    public class HitController : ApiController
    {
        readonly IGeoLocationService _geoLocationService;
        readonly IHitClientService _hitClient;

        public HitController(
            IGeoLocationService geoLocationService,
            IHitClientService hitClient)
        {
            _geoLocationService = geoLocationService;
            _hitClient = hitClient;
        }

        [Route("hit/{id}")]
        public async Task Get(string id)
        {
            var fromIPAddress = GetRemoteClientIPAddress(Request);

            var model = new HitModel
            {
                FromIP = new IPModel
                {
                    Address = fromIPAddress,
                    Location = await _geoLocationService.ExecuteAsync(fromIPAddress)
                },
                ToClientId = id
            };

            await _hitClient.ExecuteAsync(model);
        }

        static string GetRemoteClientIPAddress(
            HttpRequestMessage request)
        {
            const string key = "MS_HttpContext";
            return request.Properties.ContainsKey(key)
                ? ((HttpContextWrapper) request.Properties[key]).Request.UserHostAddress
                : null;
        }
    }
}