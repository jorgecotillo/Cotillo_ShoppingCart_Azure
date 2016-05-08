using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cotillo_ShoppingCart_Azure.Controllers
{
    /// <summary>
    /// Dummy controller to verify if the user's credentials are still valid
    /// </summary>
    public class AliveController : ApiController
    {
        /// <summary>
        /// Method used by the Mobile device to verify if the cached credentials are still valid, it they are, this call will return pong
        /// if it is no longer valid then the mobile device could not access the service requiring a credential refresh
        /// </summary>
        /// <returns>pong as a string</returns>
        [Authorize]
        public string Get()
        {
            return "pong";
        }
    }
}