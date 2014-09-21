using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sandbox.HitMe.Portal.Domain.Models;

namespace Sandbox.HitMe.Portal.Domain
{
    public class GeoLocationService : IGeoLocationService
    {
        const string ServiceUrl = "http://freegeoip.net/json/{0}";

        public async Task<LocationModel> ExecuteAsync(string ipAddress)
        {
            if (ipAddress == null
                || ipAddress.Length < 7)
                return LocationModel.Origin;

            var url = string.Format(ServiceUrl, ipAddress);
            var webClient = new WebClient();

            var response = await webClient.DownloadStringTaskAsync(url);

            dynamic location = JsonConvert.DeserializeObject(response);

            return new LocationModel
            {
                Latitude = location.latitude,
                Longitude = location.longitude
            };
        }
    }
}