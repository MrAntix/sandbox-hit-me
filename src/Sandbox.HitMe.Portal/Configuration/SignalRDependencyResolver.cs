using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;
using Microsoft.AspNet.SignalR;

namespace Sandbox.HitMe.Portal.Configuration
{
    public class SignalRDependencyResolver :
        DefaultDependencyResolver
    {
        readonly IWindsorContainer _container;

        public SignalRDependencyResolver(
            IWindsorContainer container)
        {
            _container = container;
        }

        public override object GetService(Type serviceType)
        {
            return _container.Kernel.HasComponent(serviceType)
                ? _container.Resolve(serviceType)
                : base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.ResolveAll(serviceType)
                .Cast<object>()
                .Concat(base.GetServices(serviceType));
        }
    }
}