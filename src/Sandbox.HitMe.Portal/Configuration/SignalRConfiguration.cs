using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Owin;
using Sandbox.HitMe.Portal.Realtime;

namespace Sandbox.HitMe.Portal.Configuration
{
    public static class SignalRConfiguration
    {
        public static IWindsorContainer Configure(
            this IWindsorContainer container,
            HubConfiguration configuration,
            IAppBuilder app)
        {
            configuration.Resolver = new SignalRDependencyResolver(container);

            container.Register(
                Component
                    .For<IHubConnectionContext<dynamic>>()
                    .UsingFactoryMethod(k =>
                        configuration.Resolver.Resolve<IConnectionManager>()
                            .GetHubContext<ClientsHub>().Clients)
                );

            container.Register(
                Classes
                    .FromAssemblyContaining<Startup>()
                    .BasedOn<IHub>()
                    .WithServiceSelf()
                    .LifestyleSingleton()
                );


            app.MapSignalR(configuration);

            return container;
        }
    }
}