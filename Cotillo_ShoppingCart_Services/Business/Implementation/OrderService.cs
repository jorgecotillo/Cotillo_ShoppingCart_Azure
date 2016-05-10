using Cotillo_ShoppingCart_Services.Business.Interface;
using Cotillo_ShoppingCart_Services.Domain.Model.Customer;
using Cotillo_ShoppingCart_Services.Domain.Model.Order;
using Cotillo_ShoppingCart_Services.Integration.Interfaces.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Business.Implementation
{
    public class OrderService : CRUDCommonService<OrderEntity>, IOrderService
    {
        readonly IRepository<OrderEntity> _orderRepository;
        readonly IPaymentService _paymentService;
        public OrderService(IRepository<OrderEntity> orderRepository, IPaymentService paymentService)
            :base(orderRepository)
        {
            _orderRepository = orderRepository;
            _paymentService = paymentService;
        }

        public async Task<PaymentInfoResult> CheckoutAsync(OrderEntity order, PaymentInfo paymentInfo)
        {
            try
            {
                double total = 0.0;
                //Get all order items and proceed to calculate the TotalIncTax (TotalExcTax is not included yet)
                foreach (var item in order.OrderItems)
                {
                    total += item.PriceIncTax;
                }

                order.TotalIncTax = total;

                //Now let's make the request to Paypal
                PaymentInfoResult result = await _paymentService.ProcessExternalPayment(paymentInfo);

                if(result == PaymentInfoResult.Success)
                {
                    await _orderRepository.InsertAsync(order, autoCommit: true);
                }
                else if(result == PaymentInfoResult.Failure)
                {
                    //Log failure
                }
                else
                {
                    throw new Exception("Invalid processing status received from Paypal.");
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
