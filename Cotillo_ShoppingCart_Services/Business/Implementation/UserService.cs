using Cotillo_ShoppingCart_Services.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cotillo_ShoppingCart_Services.Domain.Model.User;
using Cotillo_ShoppingCart_Services.Integration.Interfaces.EF;
using Microsoft.AspNet.Identity;

namespace Cotillo_ShoppingCart_Services.Business.Implementation
{
    public class UserService : CRUDCommonService<UserEntity>, IUserService
    {
        readonly IRepository<UserEntity> _userRepository;
        public UserService(IRepository<UserEntity> userRepository)
            :base(userRepository)
        {
            _userRepository = userRepository;
        }

        public void CreateUser(UserEntity user)
        {
            try
            {
                PasswordHasher passwordHasher = new PasswordHasher();
                string hashedPassword = passwordHasher.HashPassword(user.Password);
                user.Password = hashedPassword;
                _userRepository.Insert(user, autoCommit: true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CreateUserAsync(UserEntity user)
        {
            try
            {
                PasswordHasher passwordHasher = new PasswordHasher();
                string hashedPassword = passwordHasher.HashPassword(user.Password);
                user.Password = hashedPassword;
                await _userRepository.InsertAsync(user, autoCommit: true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePassword(int userId, string clearTextPassword)
        {
            try
            {
                PasswordHasher passwordHasher = new PasswordHasher();

                UserEntity user = base.GetById(userId);

                user.Password = passwordHasher.HashPassword(clearTextPassword);

                _userRepository.Update(user, autoCommit: true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdatePasswordAsync(int userId, string clearTextPassword)
        {
            try
            {
                PasswordHasher passwordHasher = new PasswordHasher();

                UserEntity user = await base.GetByIdAsync(userId);

                user.Password = passwordHasher.HashPassword(clearTextPassword);

                await _userRepository.UpdateAsync(user, autoCommit: true);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
