using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Model.Product
{
    public class CategoryEntity : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<ProductEntity> Products { get; set; }
    }
}
