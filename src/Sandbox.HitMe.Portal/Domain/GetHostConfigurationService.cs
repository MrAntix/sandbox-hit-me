using System;
using System.Threading.Tasks;
using System.Web;
using Sandbox.HitMe.Portal.Domain.Models;

namespace Sandbox.HitMe.Portal.Domain
{
    public class GetHostConfigurationService : IGetHostConfigurationService
    {
        HostConfigurationModel _cached;

        public async Task<HostConfigurationModel> ExecuteAsync()
        {
            return _cached ?? (_cached = GetConfiguration());
        }

        HostConfigurationModel GetConfiguration()
        {
            return new HostConfigurationModel
            {
                RootUrl = HttpContext.Current.Request
                    .Url.GetLeftPart(UriPartial.Authority)
            };
        }
    }
}