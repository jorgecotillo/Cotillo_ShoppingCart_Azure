using Cotillo_ShoppingCart_Services.Domain.Model.Customer;
using Cotillo_ShoppingCart_Services.Domain.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Business.Interface
{
    public interface IOrderService : ICRUDCommonService<OrderEntity>
    {
        Task<PaymentInfoResult> CheckoutAsync(OrderEntity order, PaymentInfo paymentInfo);
    }
}
