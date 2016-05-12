using Cotillo_ShoppingCart_Services.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public int CustomerId { get; set; }
        /// <summary>
        /// Comma separated roles
        /// </summary>
        public ICollection<RoleEntity> Roles { get; set; }
        public string Password { get; internal set; }
    }
}
