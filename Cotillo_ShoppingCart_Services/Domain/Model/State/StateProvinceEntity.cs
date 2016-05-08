using Cotillo_ShoppingCart_Services.Domain.Model.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Model.State
{
    public class StateProvinceEntity : BaseEntity
    {
        public string Name { get; set; }

        public int CountryId { get; set; }
        public virtual CountryEntity Country { get; set; }
    }
}
