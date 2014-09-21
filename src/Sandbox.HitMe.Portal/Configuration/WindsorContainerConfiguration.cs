using System.Collections.Concurrent;
using Antix.Logging;
using Antix.Services;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Sandbox.HitMe.Portal.Domain.Models;

namespace Sandbox.HitMe.Portal.Configuration
{
    public static class WindsorContainerConfiguration
    {
        public static IWindsorContainer Configure(
            this IWindsorContainer container)
        {
            RegisterLogging(container);
            RegisterServices(container);

            return container;
        }

        static void RegisterLogging(IWindsorContainer container)
        {
            container.Register(
                Component.For<Log.Delegate>()
                    .Instance(Log.ToConsole)
                    .LifestyleSingleton()
                );
        }

        static void RegisterServices(IWindsorContainer container)
        {
            container.Register(
                Component
                    .For<ConcurrentDictionary<string, ClientModel>>()
                    .LifestyleSingleton()
                );

            container.Register(
                Classes
                    .FromAssemblyContaining<Startup>()
                    .BasedOn<IService>()
                    .WithServiceAllInterfaces()
                    .WithServiceSelf()
                    .LifestyleTransient()
                );
        }
    }
}