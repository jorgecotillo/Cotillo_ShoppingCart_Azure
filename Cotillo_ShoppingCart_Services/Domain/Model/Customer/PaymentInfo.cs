using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Model.Customer
{
    public class PaymentInfo
    {
        public string CreditCardNo { get; set; }
        public int CVV2 { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    public enum PaymentInfoResult
    {
        None = 0,
        Success = 1,
        Failure = 2
    }
}
