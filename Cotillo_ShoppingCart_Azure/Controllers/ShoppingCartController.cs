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

namespace Cotillo_ShoppingCart_Azure.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/v1/shopping-cart")]
    public class ShoppingCartController : ApiController
    {
        readonly IShoppingCartService _shoppingCartService;
        readonly IProductService _productService;
        
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
        public async Task<IHttpActionResult> AddShoppingCartItem(string customerId, [FromBody] ShoppingCartModel model)
        {
            try
            {
                ProductEntity product = null;
                ShoppingCartItemEntity shoppingCartItem = null;
                product = await _productService.GetByIdAsync(model.ProductId);
                shoppingCartItem = new ShoppingCartItemEntity()
                {
                    Active = true,
                    CreatedOn = DateTime.Now,
                    CustomerId = int.Parse(customerId),
                    LastUpdated = DateTime.Now,
                    ProductId = model.ProductId,
                    PriceIncTax = product.PriceIncTax
                };
                await _shoppingCartService.SaveAsync(shoppingCartItem, autoCommit: true);
                return Ok();
            }
            catch (Exception)
            {
                return new InternalServerErrorResult(Request);
            }
            
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Post(string customerId, [FromBody] List<ShoppingCartModel> model)
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
                return new InternalServerErrorResult(Request);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> RemoveFromCart()
        {
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IHttpActionResult> ListCart()
        {
            return Ok();
        }
    }
}
