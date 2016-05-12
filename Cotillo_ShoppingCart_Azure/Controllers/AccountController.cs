using Cotillo_ShoppingCart_Models;
using Cotillo_ShoppingCart_Services.Business.Interface;
using Cotillo_ShoppingCart_Services.Domain.Model.User;
using Microsoft.Azure.Mobile.Server.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using Cotillo_ShoppingCart_Azure.Models;
using Cotillo_ShoppingCart_Services.Domain.Model.Customer;
using Cotillo_ShoppingCart_Services.Domain.Model.Addresses;

namespace Cotillo_ShoppingCart_Azure.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/v1/account")]
    public class AccountController : ApiController
    {
        readonly IUserService _userService;
        readonly IMessageService _messageService;
        readonly ICustomerService _customerService;
        public AccountController(IUserService userService, IMessageService messageService, ICustomerService customerService)
        {
            _userService = userService;
            _messageService = messageService;
            _customerService = customerService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("external")]
        public async Task<IHttpActionResult> RegisterExternalLogin([FromBody] RegisterExternalModel model)
        {
            try
            {
                //Hard coding State and Country for now, it should come from the customer registration
                var customer = new CustomerEntity()
                {
                    Active = true,
                    BillingAddress = new AddressEntity()
                    {
                        Active = true,
                        Address1 = "70 Blanchard Rd",
                        Address2 = "Suite 500",
                        AddressType = 1,
                        CountryId = 1,
                        CreatedOn = DateTime.Now,
                        LastUpdated = DateTime.Now,
                        StateProvinceId = 1
                    },
                    CreatedOn = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    User = new UserEntity()
                    {
                        Active = true,
                        CreatedOn = DateTime.Now,
                        LastUpdated = DateTime.Now,
                        Password = "",
                        DisplayName = model.Name,
                        UserName = model.Username,
                        ExternalAccount = model.ExternalAccount
                    }
                };

                await
                    _customerService.SaveAsync(customer, autoCommit: true);

                //Let's queue the email
                _messageService.QueueEmail(new Cotillo_ShoppingCart_Services.Domain.Model.Message.EmailEntity()
                {
                    Subject = "Welcome to the store!",
                    Body = "This email confirms that your account got successfully registered",
                    From = "no-reply@mystore.com",
                    To = model.Username,
                    CC = new List<string>() { "jorge.cotillo@gmail.com" }
                });
                return Ok(new CustomerModel { CustomerId = customer.Id });
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
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

        [Route("external-user/{userId}")]
        [HttpGet]
        public async Task<HttpResponseMessage> ExternalUserExists(string userId)
        {
            try
            {
                //Verify if the user exists in the database, if not return an error (Not Found)
                userId = $"sid:{userId}";
                //using an already evaluated variable otherwise LINQ will try to evaluate the format and will throw an exception (lazy evaluation)
                Expression<Func<UserEntity, bool>> where = i => i.ExternalAccount == userId;

                var user = await _userService.GetByFilterAsync(where);

                if (user == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent("No user")
                    };
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(user.ToUserInfoModel()))
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[Authorize]
        [Route("external-info")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetExternalUserInfo()
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
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(extendedUserInfo))
                    };
                else
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
