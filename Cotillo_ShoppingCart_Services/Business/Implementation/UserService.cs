using Cotillo_ShoppingCart_Services.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cotillo_ShoppingCart_Services.Domain.Model.User;
using Cotillo_ShoppingCart_Services.Integration.Interfaces.EF;
using Microsoft.AspNet.Identity;
using System.Linq.Expressions;
using LinqKit;
using System.Data.Entity;

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

        public async Task<UserEntity> GetByFilterAsync(Expression<Func<UserEntity, bool>> where)
        {
            try
            {
                //Using AsExpandable from Linkit so that EF (Linq To Entities) can resolve in runtime the dynamic filter,
                //otherwise Linq To Entities will throw an exception.
                return
                    await _userRepository
                        .Table
                        .AsExpandable()
                        .Where(where)
                        .Select(i => i)
                        .FirstOrDefaultAsync();
            }
            catch (Exception ex)
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
