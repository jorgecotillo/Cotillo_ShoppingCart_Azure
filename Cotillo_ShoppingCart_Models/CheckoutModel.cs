using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Models
{
    public class CheckoutModel
    {
        public IList<int> ProductIds { get; set; }
        public PaymentInfoModel PaymentInfoModel { get; set; }
    }

    public class PaymentInfoModel
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string CreditCardNo { get; set; }
        public DateTime ExpireDate { get; set; }
        public int CVV2 { get; set; }
    }
}
