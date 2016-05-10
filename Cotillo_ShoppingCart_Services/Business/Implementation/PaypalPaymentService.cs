using Cotillo_ShoppingCart_Services.Business.Interface;
using Cotillo_ShoppingCart_Services.Domain.Model.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Business.Implementation
{
    public class PaypalPaymentService : IPaymentService
    {
        public async Task<PaymentInfoResult> ProcessExternalPayment(PaymentInfo paymentInfo)
        {
            return PaymentInfoResult.Success;
        }
    }
}
