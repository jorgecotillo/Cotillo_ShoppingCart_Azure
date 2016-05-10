using Cotillo_ShoppingCart_Models;
using Cotillo_ShoppingCart_Services.Business.Interface;
using Cotillo_ShoppingCart_Services.Domain.Model.Customer;
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
    [RoutePrefix("api/v1/orders")]
    public class OrdersController : ApiController
    {
        readonly IOrderService _orderService;
        readonly IProductService _productService;
        public OrdersController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("checkout/{customerId}")]
        public async Task<IHttpActionResult> Checkout(int customerId, [FromBody] CheckoutModel checkoutModel)
        {
            try
            {
                OrderEntity order = new OrderEntity();
                order.OrderItems = new List<OrderItemEntity>();
                OrderItemEntity orderItem = null;
                ProductEntity product = null;
                PaymentInfo payment = null;
                foreach (var item in checkoutModel.ProductIds)
                {
                    product = await _productService.GetByIdAsync(item);

                    orderItem = new OrderItemEntity()
                    {
                        Active = true,
                        CreatedOn = DateTime.Now,
                        LastUpdated = DateTime.Now,
                        PriceIncTax = product.PriceIncTax,
                        ProductId = item
                    };

                    order.OrderItems.Add(orderItem);
                }

                payment = new PaymentInfo()
                {
                    Address = "",
                    City = "",
                    Country = "",
                    CreditCardNo = checkoutModel.PaymentInfoModel.CreditCardNo,
                    CVV2 = checkoutModel.PaymentInfoModel.CVV2
                };
                await _orderService.CheckoutAsync(order, payment);
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
        /// <returns></returns>
        public async Task<IHttpActionResult> ListOrders()
        {
            return Ok();
        }
    }
}
