using Cotillo_ShoppingCart_Services.Domain.Model.Customer;
using Cotillo_ShoppingCart_Services.Domain.Model.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Model.Order
{
    public class ShoppingCartItemEntity : BaseEntity
    {
        public int ProductId { get; set; }
        public virtual ProductEntity Product { get; set; }
        public int CustomerId { get; set; }
        public virtual CustomerEntity Customer { get; set; }
        public double PriceIncTax { get; set; }
        public double? PriceExcTax { get; set; }
    }
}
