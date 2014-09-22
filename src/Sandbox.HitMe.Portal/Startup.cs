using System.Web.Http;
using System.Web.Optimization;
using Castle.Windsor;
using Microsoft.AspNet.SignalR;
using Owin;
using Sandbox.HitMe.Portal.Configuration;

namespace Sandbox.HitMe.Portal
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            BundleConfiguration.Configure(BundleTable.Bundles);
           // BundleTable.EnableOptimizations = false;

            new WindsorContainer()
                .Configure()
                .Configure(new HubConfiguration(), app)
                .Configure(GlobalConfiguration.Configuration);
        }
    }
}