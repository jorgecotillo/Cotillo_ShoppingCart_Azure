using Cotillo_ShoppingCart_Azure.Models;
using Cotillo_ShoppingCart_Models;
using Cotillo_ShoppingCart_Services.IoCContainer;
using Microsoft.Azure.Mobile.Server.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cotillo_ShoppingCart_Azure.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ValuesController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<HttpResponseMessage> Get()
        {
            

            return Request.CreateResponse(HttpStatusCode.OK, new string[] { "value1", "value2" });
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
