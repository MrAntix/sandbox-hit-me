using System.Threading.Tasks;
using System.Web.Http;
using Antix.Http.Filters.Logging;
using Sandbox.HitMe.Portal.Domain;
using Sandbox.HitMe.Portal.Domain.Models;

namespace Sandbox.HitMe.Portal.Api
{
    [LogAction]
    public class SendController : ApiController
    {
        readonly ISendService _sendService;

        public SendController(ISendService sendService)
        {
            _sendService = sendService;
        }

        [Route("api/send")]
        public async Task Post (SendModel model)
        {
            await _sendService.ExecuteAsync(model);
        }
    }
}