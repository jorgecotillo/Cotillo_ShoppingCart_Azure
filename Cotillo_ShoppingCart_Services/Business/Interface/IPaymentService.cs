﻿using Cotillo_ShoppingCart_Services.Domain.Model.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Business.Interface
{
    public interface IPaymentService
    {
        Task<PaymentInfoResult> ProcessExternalPayment(PaymentInfo paymentInfo);
    }
}
