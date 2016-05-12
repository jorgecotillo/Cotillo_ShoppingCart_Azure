using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Model.User
{
    public class RoleEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Key { get; set; }
        [JsonIgnore] //Ignoring this property otherwise JsonConver will get into a loop from Users to Roles and from Roles to Users and so on
        public virtual ICollection<UserEntity> Users { get; set; }
    }
}
