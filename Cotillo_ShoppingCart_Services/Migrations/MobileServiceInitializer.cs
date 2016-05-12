using Cotillo_ShoppingCart_Services.Domain.Model.Addresses;
using Cotillo_ShoppingCart_Services.Domain.Model.Country;
using Cotillo_ShoppingCart_Services.Domain.Model.Customer;
using Cotillo_ShoppingCart_Services.Domain.Model.Product;
using Cotillo_ShoppingCart_Services.Domain.Model.State;
using Cotillo_ShoppingCart_Services.Domain.Model.User;
using Cotillo_ShoppingCart_Services.Integration.Implementation.EF;
using Cotillo_ShoppingCart_Services.Integration.Interfaces.EF;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Cotillo_ShoppingCart_Services.Migrations
{
    /// <summary>
    /// 
    /// </summary>
    public class MobileServiceInitializer : CreateDatabaseIfNotExists<EFContext>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void InitializeDatabase(EFContext context)
        {
            base.InitializeDatabase(context);
        }

        protected override void Seed(EFContext context)
        {
            try
            {
                //Product Seed Data

                IRepository<ProductEntity> productRepository = new EFRepository<ProductEntity>(context);
                //Set the value to compare with
                Expression<Func<ProductEntity, object>> productFilter = i => i.Barcode;

                IRepository<CategoryEntity> categoryRepository = new EFRepository<CategoryEntity>(context);
                //Set the value to compare with
                Expression<Func<CategoryEntity, object>> categoryFilter = i => i.Name;

                var categoryEntity = new CategoryEntity()
                {
                    Active = true,
                    CreatedOn = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    Location = "Aisle 2, next to Frozen Yogurt section",
                    Name = "Milk",
                    Products = new List<ProductEntity>()
                {
                    new ProductEntity()
                    {
                        Active = true,
                        CreatedOn = DateTime.Now,
                        FileName = "horizon_dha.png",
                        LastUpdated = DateTime.Now,
                        Name = "Horizon Milk",
                        Barcode = "9780553061727",
                        PriceIncTax = 2.90,
                        PriceExcTax = 2.79,
                        Description = "Organic Milk that contains DHA, perfect for your kids!",
                        Location = "Aisle 2, next to Frozen Yogurt section",
                        ExpiresOn = DateTime.Now.AddDays(15)
                    },
                    new ProductEntity()
                    {
                        Active = true,
                        CreatedOn = DateTime.Now,
                        FileName = "silk.png",
                        LastUpdated = DateTime.Now,
                        Name = "Silk Milk",
                        Barcode = "720473600209",
                        PriceIncTax = 2.99,
                        PriceExcTax = 2.70,
                        Description = "Soy Milk, great with your breakfast everyday, good source of Vitamin D",
                        Location = "Aisle 2, next to Frozen Yogurt section",
                        ExpiresOn = DateTime.Now.AddDays(15)
                    }
                }
                };

                categoryRepository.AddOrUpdate(categoryFilter, categoryEntity);

                IRepository<CountryEntity> countryRepository = new EFRepository<CountryEntity>(context);
                Expression<Func<CountryEntity, object>> countryFilter = i => i.Name;

                CountryEntity country = new CountryEntity()
                {
                    Active = true,
                    CreatedOn = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    Name = "USA"
                };

                countryRepository.AddOrUpdate(countryFilter, country);


                IRepository<StateProvinceEntity> stateRepository = new EFRepository<StateProvinceEntity>(context);
                //Set the value to compare with
                Expression<Func<StateProvinceEntity, object>> stateFilter = i => i.Name;

                StateProvinceEntity state = new StateProvinceEntity()
                {
                    Active = true,
                    CreatedOn = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    Name = "MA",
                    CountryId = country.Id
                };
                stateRepository.AddOrUpdate(stateFilter, state);

                IRepository<RoleEntity> roleRepository = new EFRepository<RoleEntity>(context);

                //Set the value to compare with
                Expression<Func<RoleEntity, object>> roleFilter = i => i.Key;

                RoleEntity roleAdmin = new RoleEntity()
                {
                    Active = true,
                    CreatedOn = DateTime.Now,
                    Key = "Admin",
                    LastUpdated = DateTime.Now,
                    Name = "Administrator Group"
                };

                roleRepository.AddOrUpdate(roleFilter, roleAdmin);

                RoleEntity roleUser = new RoleEntity()
                {
                    Active = true,
                    CreatedOn = DateTime.Now,
                    Key = "User",
                    LastUpdated = DateTime.Now,
                    Name = "Users Group"
                };

                roleRepository.AddOrUpdate(roleFilter, roleUser);

                IRepository<UserEntity> userRepository = new EFRepository<UserEntity>(context);

                //Set the value to compare with
                Expression<Func<UserEntity, object>> adminUserFilter = i => i.UserName;
                PasswordHasher hasher = new PasswordHasher();

                UserEntity user = new UserEntity()
                {
                    Active = true,
                    CreatedOn = DateTime.Now,
                    DisplayName = "Admin User",
                    LastUpdated = DateTime.Now,
                    Password = hasher.HashPassword("collab"),
                    UserName = "jorge.cotillo@gmail.com",
                    ExternalAccount = "jorge.cotillo@gmail.com",
                    Roles = new List<RoleEntity>() { roleAdmin } //Referencing the whole entity and not just the Id (safe to do so, it won't create duplicates)
                };

                userRepository.AddOrUpdate(adminUserFilter, user);

                IRepository<AddressEntity> addressRepository = new EFRepository<AddressEntity>(context);
                //Set the value to compare with
                Expression<Func<AddressEntity, object>> adminAddressFilter = i => i.Id;
                AddressEntity adminAddress = new AddressEntity()
                {
                    Active = true,
                    Address1 = "70 Blanchard Rd",
                    Address2 = "Suite 500",
                    AddressType = 1,
                    Country = country,
                    CreatedOn = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    StateProvince = state
                };

                addressRepository.AddOrUpdate(adminAddress);

                IRepository<CustomerEntity> customerRepository = new EFRepository<CustomerEntity>(context);
                //Set the value to compare with
                Expression<Func<CustomerEntity, object>> customerFilter = i => i.Email;

                CustomerEntity customer = new CustomerEntity()
                {
                    Active = true,
                    Email = "jorge.cotillo@gmail.com",
                    BillingAddress = adminAddress,
                    CreatedOn = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    User = user
                };

                customerRepository.AddOrUpdate(customerFilter, customer);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + "BREAK " + ex.InnerException?.Message);
            }
        }
    }
}
