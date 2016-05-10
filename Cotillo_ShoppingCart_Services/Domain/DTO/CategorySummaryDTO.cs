using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.DTO
{
    public class CategorySummaryDTO
    {
        public string CategoryName { get; set; }
        public int ProductCount { get; set; }
    }
}
