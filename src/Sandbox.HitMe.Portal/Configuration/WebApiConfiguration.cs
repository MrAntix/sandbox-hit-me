using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Filters;
using Antix.Http.Dispatcher;
using Antix.Http.Filters;
using Antix.Http.Filters.Logging;
using Antix.Logging;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Newtonsoft.Json.Serialization;

namespace Sandbox.HitMe.Portal.Configuration
{
    public static class WebApiConfiguration
    {
        public static IWindsorContainer Configure(
            this IWindsorContainer container,
            HttpConfiguration configuration)
        {
            configuration.MapHttpAttributeRoutes();

            var formatter = configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver
                = new CamelCasePropertyNamesContractResolver();

            configuration.Formatters.Clear();
            configuration.Formatters.Add(formatter);

            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn<ApiController>()
                    .LifestyleTransient()
                );

            configuration.Services.Replace(
                typeof (IHttpControllerActivator),
                new ServiceHttpControllerActivator(
                    t => (IHttpController) container.Resolve(t),
                    container.Release,
                    container.Resolve<Log.Delegate>())
                );

            configuration.Services.Replace(
                typeof (IFilterProvider),
                new ServiceFilterProvider(container.Resolve)
                );
            container.Register(
                Classes
                    .FromAssemblyContaining<LogActionFilter>()
                    .BasedOn<IFilter>()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces()
                    .LifestyleTransient()
                );

            configuration.EnsureInitialized();

            return container;
        }
    }
}