using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Models
{
    public class CategorySummaryModel
    {
        public string CategoryName { get; set; }
        public string Location { get; set; }
        public int ProductCount { get; set; }
    }
}
