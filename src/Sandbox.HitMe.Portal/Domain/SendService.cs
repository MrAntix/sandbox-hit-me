using System.Net.Mail;
using System.Threading.Tasks;
using Antix.Logging;
using Sandbox.HitMe.Portal.Domain.Models;

namespace Sandbox.HitMe.Portal.Domain
{
    public class SendService :
        ISendService
    {
        readonly Log.Delegate _log;
        readonly IGetHostConfigurationService _getHostConfigurationService;

        public SendService(
            Log.Delegate log,
            IGetHostConfigurationService getHostConfigurationService)
        {
            _log = log;
            _getHostConfigurationService = getHostConfigurationService;
        }

        public async Task ExecuteAsync(SendModel model)
        {
            var smtp = new SmtpClient();
            var config = await _getHostConfigurationService.ExecuteAsync();

            var message = new MailMessage
            {
                Subject = "Your message from hit.antix.co.uk",
                Body = string.Format(
                    "Please click the link {0}{1}{2}",
                    config.RootUrl, "/hit/",
                    model.ClientConnectionId)
            };

            message.To.Add(new MailAddress(model.Email));

            _log.Information(m => m("Sending Email: {0}", model.Email));
            smtp.Send(message);
        }
    }
}