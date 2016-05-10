using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Model.Product
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public string FileName { get; set; }
        public string Barcode { get; set; }
        public DateTime ExpiresOn { get; set; }
        public int CategoryId { get; set; }
        public virtual CategoryEntity Category { get; set; }
        public double PriceIncTax { get; set; }
        public double PriceExcTax { get; set; }
    }
}
