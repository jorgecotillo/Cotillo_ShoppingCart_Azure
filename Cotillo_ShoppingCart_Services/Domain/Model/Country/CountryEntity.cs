using Cotillo_ShoppingCart_Services.Domain.Model.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Model.Country
{
    public class CountryEntity : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<StateProvinceEntity> StateProvinces { get; set; }
    }
}
