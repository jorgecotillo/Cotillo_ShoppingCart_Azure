using Microsoft.AspNet.Identity;

namespace Cotillo_ShoppingCart_Services.Domain.Model.User
{
    public class UserEntity : BaseEntity, IUser<int>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
