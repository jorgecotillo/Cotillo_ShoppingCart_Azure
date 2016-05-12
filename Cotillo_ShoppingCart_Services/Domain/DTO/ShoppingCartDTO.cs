using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.DTO
{
    public class ShoppingCartDTO
    {
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public int ShoppingCartId { get; set; }
        public int Quantity { get; set; }
        public double PriceIncTax { get; set; }
    }
}
