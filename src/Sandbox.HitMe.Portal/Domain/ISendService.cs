using Antix.Services;
using Sandbox.HitMe.Portal.Domain.Models;

namespace Sandbox.HitMe.Portal.Domain
{
    public interface ISendService :
        IServiceIn<SendModel>
    {
    }
}