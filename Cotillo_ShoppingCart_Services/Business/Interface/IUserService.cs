using Cotillo_ShoppingCart_Services.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Business.Interface
{
    public interface IUserService : ICRUDCommonService<UserEntity>
    {
        void CreateUser(UserEntity user);
        Task CreateUserAsync(UserEntity user);
    }
}
