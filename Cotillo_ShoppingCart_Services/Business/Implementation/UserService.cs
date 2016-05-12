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
using Cotillo_ShoppingCart_Services.Domain.DTO;
using Cotillo_ShoppingCart_Services.Domain.Model.Customer;

namespace Cotillo_ShoppingCart_Services.Business.Implementation
{
    public class UserService : CRUDCommonService<UserEntity>, IUserService
    {
        readonly IRepository<UserEntity> _userRepository;
        readonly IRepository<CustomerEntity> _customerRepository;
        readonly IRepository<RoleEntity> _roleRepository;
        public UserService(
            IRepository<UserEntity> userRepository, 
            IRepository<CustomerEntity> customerRepository,
            IRepository<RoleEntity> roleRepository)
            :base(userRepository)
        {
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _roleRepository = roleRepository;
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

        public async Task<UserDTO> GetByFilterAsync(Expression<Func<UserEntity, bool>> where)
        {
            try
            {
                //Using AsExpandable from Linkit so that EF (Linq To Entities) can resolve in runtime the dynamic filter,
                //otherwise Linq To Entities will throw an exception.

                var userQueryable = _userRepository
                                    .Table
                                    .AsExpandable()
                                    .Where(where)
                                    .Select(i => i);

                var userFiltered = await (from customer in _customerRepository.Table
                                          join user in userQueryable on customer.UserId equals user.Id
                                          select new UserDTO()
                                          {
                                              CustomerId = customer.Id,
                                              DisplayName = user.DisplayName,
                                              UserId = user.Id,
                                              UserName = user.UserName,
                                              Roles = user.Roles
                                          }).FirstOrDefaultAsync();

                return userFiltered;
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

        public async Task AssociateUserToRoleAsync(int userId, int roleId, string roleKey = "")
        {
            try
            {
                //Verify this method because is going multiple times to the DB, it should work by adding a new item in the list and specify
                //the Id (Role.Id) probably the scope of the DB Context
                var user = await _userRepository
                    .Table
                    .Where(i => i.Id == userId)
                    .FirstOrDefaultAsync();

                RoleEntity role = null;

                if (!string.IsNullOrWhiteSpace(roleKey))
                    role = await _roleRepository.Table.Where(i => i.Id == roleId).FirstOrDefaultAsync();
                else
                    role = await _roleRepository.Table.Where(i => i.Key == roleKey).FirstOrDefaultAsync();

                if (role == null)
                    throw new ArgumentException("Invalid Role");

                user.Roles.Add(role);

                await _userRepository.UpdateAsync(user, autoCommit: true);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserEntity> GetByCustomerIdAsync(int customerId)
        {
            try
            {
                return await (from user in _userRepository.Table
                              join customer in _customerRepository.Table on user.Id equals customer.UserId
                              select user).FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserDTO> GetByUsernameAsync(string username)
        {
            try
            {
                return await (from customer in _customerRepository.Table
                              join user in _userRepository.Table on customer.UserId equals user.Id
                              where user.UserName == username
                              select new UserDTO()
                              {
                                  CustomerId = customer.Id,
                                  DisplayName = user.DisplayName,
                                  UserId = user.Id,
                                  UserName = user.UserName,
                                  Password = user.Password,
                                  Roles = user.Roles
                              }).FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
