using Antix.Services;
using Sandbox.HitMe.Portal.Domain.Models;

namespace Sandbox.HitMe.Portal.Domain
{
    public interface IGeoLocationService :
        IServiceInOut<string, LocationModel>
    {
    }
}