using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Sandbox.HitMe.Portal.Server;

namespace Sandbox.HitMe.Portal.Api
{
    public class HitController : ApiController
    {
        [Route("hit/{id}")]
        public void Get(string id)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ClientsHub>();

            context.Clients.All.Hit(new
            {
                from= "hi",
                to = id
            });
        }
    }
}