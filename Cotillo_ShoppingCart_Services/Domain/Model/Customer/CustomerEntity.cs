using Cotillo_ShoppingCart_Services.Domain.Model.Addresses;
using Cotillo_ShoppingCart_Services.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Model.Customer
{
    public class CustomerEntity : BaseEntity
    {
        public string Email { get; set; }
        public int BillingAddressId { get; set; }
        public virtual AddressEntity BillingAddress { get; set; }
        public int UserId { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
