using Cotillo_ShoppingCart_Services.Domain.Model.Country;
using Cotillo_ShoppingCart_Services.Domain.Model.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Model.Addresses
{
    public class AddressEntity : BaseEntity
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int AddressType { get; set; }
        public int CountryId { get; set; }
        public virtual CountryEntity Country { get; set; }
        public int StateProvinceId { get; set; }
        public virtual StateProvinceEntity StateProvince { get; set; }
    }
}