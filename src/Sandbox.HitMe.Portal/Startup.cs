using System.Web.Http;
using System.Web.Optimization;
using Owin;
using Sandbox.HitMe.Portal.Configuration;

namespace Sandbox.HitMe.Portal
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            BundleConfiguration.Configure(BundleTable.Bundles);
        }
    }
}