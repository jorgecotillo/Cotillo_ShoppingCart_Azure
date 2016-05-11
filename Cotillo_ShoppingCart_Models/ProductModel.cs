using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        public string Description { get; set; }
        public double PriceIncTax { get; set; }
        public double PriceExcTax { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte[] Image { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
