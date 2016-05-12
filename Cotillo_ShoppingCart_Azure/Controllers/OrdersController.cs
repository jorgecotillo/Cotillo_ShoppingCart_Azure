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
    [Authorize]
    public class OrdersController : ApiController
    {
        readonly IOrderService _orderService;
        readonly IProductService _productService;
        readonly IMessageService _messageService;
        readonly IUserService _userService;
        readonly ICustomerService _customerService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderService"></param>
        /// <param name="productService"></param>
        /// <param name="messageService"></param>
        /// <param name="userService"></param>
        /// <param name="customerService"></param>
        public OrdersController(
            IOrderService orderService, 
            IProductService productService,
            IMessageService messageService,
            IUserService userService,
            ICustomerService customerService)
        {
            _orderService = orderService;
            _productService = productService;
            _messageService = messageService;
            _userService = userService;
            _customerService = customerService;
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
                OrderEntity order = new OrderEntity()
                {
                    Active = true,
                    CreatedOn = DateTime.Now,
                    CreditCardNo = checkoutModel.PaymentInfoModel.CreditCardNo,
                    CustomerId = customerId,
                    LastUpdated = DateTime.Now,
                    TotalIncTax = checkoutModel.TotalIncTax 
                };
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
                    Address = checkoutModel.PaymentInfoModel.Address,
                    City = checkoutModel.PaymentInfoModel.City,
                    Country = checkoutModel.PaymentInfoModel.Country,
                    CreditCardNo = checkoutModel.PaymentInfoModel.CreditCardNo,
                    CVV2 = checkoutModel.PaymentInfoModel.CVV2
                };

                await _orderService.CheckoutAsync(order, payment);

                var user = await _userService.GetByCustomerIdAsync(customerId);

                //Let's queue the email
                _messageService.QueueEmail(new Cotillo_ShoppingCart_Services.Domain.Model.Message.EmailEntity()
                {
                    Subject = "Your new order!",
                    Body = "This email confirms your payment has been successfully processed.",
                    From = "no-reply@mystore.com",
                    To = user.UserName,
                    CC = new List<string>() { "jorge.cotillo@gmail.com" }
                });

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
