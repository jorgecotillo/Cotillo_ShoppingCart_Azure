using Cotillo_ShoppingCart_Azure.Models;
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
    public class ValuesController : ApiController
    {
        [Authorize]
        public async Task<HttpResponseMessage> Get()
        {
            // Service User is an implementation of IPrincipal 
            // Defines how the user is authenticated
            // Obtained via a Cast of the User property from the ApiController derived class

            ExtendedUserInfoModel extendedUserInfo = null;
            string providerType = null;

            // Try to get the twitter credential info
            FacebookCredentials facebookCredentials = await User.GetAppServiceIdentityAsync<FacebookCredentials>(Request);
            if (facebookCredentials.Provider == "Facebook")
            {
                // Create a query string with the Facebook access token.
                var fbRequestUrl = $"https://graph.facebook.com/me?access_token={facebookCredentials.AccessToken}";

                // Create an HttpClient request.
                using (var client = new System.Net.Http.HttpClient())
                {
                    // Request the current user info from Facebook.
                    using (var resp = await client.GetAsync(fbRequestUrl))
                    {
                        resp.EnsureSuccessStatusCode();

                        // Do something here with the Facebook user information.
                        var fbInfo = await resp.Content.ReadAsStringAsync();
                    }
                }
            }

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
