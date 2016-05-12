using Cotillo_ShoppingCart_Services.Domain.DTO;
using Cotillo_ShoppingCart_Services.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Business.Interface
{
    public interface IUserService : ICRUDCommonService<UserEntity>
    {
        void CreateUser(UserEntity user);
        Task CreateUserAsync(UserEntity user);
        Task<UserDTO> GetByFilterAsync(Expression<Func<UserEntity, bool>> where);
        Task AssociateUserToRoleAsync(int userId, int roleId, string roleKey = "");
        Task<UserEntity> GetByCustomerIdAsync(int customerId);
        Task<UserDTO> GetByUsernameAsync(string username);
    }
}
