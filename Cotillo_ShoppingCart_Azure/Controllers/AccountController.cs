﻿using Cotillo_ShoppingCart_Models;
using Microsoft.Azure.Mobile.Server.Authentication;
using Newtonsoft.Json;
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
    [RoutePrefix("api/v1/account")]
    public class AccountController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> RegisterExternalLogin()
        {
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> RegisterLogin()
        {
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> Login()
        {
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        [Route("external-info")]
        [HttpGet]
        public async Task<IHttpActionResult> GetExternalUserInfo()
        {
            try
            {
                // Service User is an implementation of IPrincipal 
                // Defines how the user is authenticated
                // Obtained via a Cast of the User property from the ApiController derived class

                ExtendedUserInfoModel extendedUserInfo = null;
                // Try to get the twitter credential info
                FacebookCredentials facebookCredentials = await User.GetAppServiceIdentityAsync<FacebookCredentials>(Request);
                if (facebookCredentials != null && facebookCredentials.Provider == "Facebook")
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

                            if (String.IsNullOrWhiteSpace(fbInfo))
                                throw new ArgumentNullException("External info is empty");

                            extendedUserInfo = JsonConvert.DeserializeObject<ExtendedUserInfoModel>(fbInfo);
                        }
                    }
                }
                if (extendedUserInfo != null)
                    return Ok(extendedUserInfo);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
