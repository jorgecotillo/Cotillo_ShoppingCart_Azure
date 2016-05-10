using Cotillo_ShoppingCart_Services.Domain.Model.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Model.Order
{
    public class OrderEntity : BaseEntity
    {
        public int CustomerId { get; set; }
        public virtual CustomerEntity Customer { get; set; }
        public double TotalIncTax { get; set; }
        public double? TotalExcTax { get; set; }
        public string CreditCardNo { get; set; }
        public virtual ICollection<OrderItemEntity> OrderItems { get; set; }
    }
}
