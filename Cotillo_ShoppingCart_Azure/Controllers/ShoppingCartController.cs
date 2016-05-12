using Cotillo_ShoppingCart_Models;
using Cotillo_ShoppingCart_Services.Business.Interface;
using Cotillo_ShoppingCart_Services.Domain.Model.Order;
using Cotillo_ShoppingCart_Services.Domain.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Cotillo_ShoppingCart_Azure.Models;

namespace Cotillo_ShoppingCart_Azure.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/v1/shopping-cart")]
    [Authorize]
    public class ShoppingCartController : ApiController
    {
        readonly IShoppingCartService _shoppingCartService;
        readonly IProductService _productService;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shoppingCartService"></param>
        /// <param name="productService"></param>
        public ShoppingCartController(IShoppingCartService shoppingCartService, IProductService productService)
        {
            _shoppingCartService = shoppingCartService;
            _productService = productService;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("customer/{customerId}")]
        public async Task<IHttpActionResult> AddAllShoppingCartItems(string customerId, [FromBody] List<ShoppingCartModel> model)
        {
            try
            {
                ProductEntity product = null;
                ShoppingCartItemEntity shoppingCartItem = null;
                foreach (var item in model)
                {
                    product = await _productService.GetByIdAsync(item.ProductId);
                    shoppingCartItem = new ShoppingCartItemEntity()
                    {
                        Active = true,
                        CreatedOn = DateTime.Now,
                        CustomerId = int.Parse(customerId),
                        LastUpdated = DateTime.Now,
                        ProductId = item.ProductId,
                        PriceIncTax = product.PriceIncTax
                    };
                    //Since is not going to the DB, no need to use the async version
                    _shoppingCartService.Save(shoppingCartItem, autoCommit: false);
                }
                //Using async version since is going to the DB
                await _shoppingCartService.CommitAsync();

                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shoppingCartId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{shoppingCartId}")]
        public async Task<IHttpActionResult> RemoveFromCart(int shoppingCartId)
        {
            try
            {
                await _shoppingCartService.DeleteByIdAsync(shoppingCartId);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("customer/{customerId}")]
        public async Task<IHttpActionResult> RemoveAllByCustomer(int customerId)
        {
            try
            {
                await _shoppingCartService.DeleteAllByCustomerIdAsync(customerId);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("customer/{customerId}")]
        public async Task<IHttpActionResult> ListCart(int customerId)
        {
            try
            {
                var shoppingCartItems = await _shoppingCartService.GetAllByCustomerIdAsync(customerId);
                return Ok(shoppingCartItems.ToShoppingCartListModel());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
