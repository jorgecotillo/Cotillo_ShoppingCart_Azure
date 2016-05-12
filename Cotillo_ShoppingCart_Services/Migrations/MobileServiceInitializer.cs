using Cotillo_ShoppingCart_Services.Domain.Model.Country;
using Cotillo_ShoppingCart_Services.Domain.Model.Product;
using Cotillo_ShoppingCart_Services.Domain.Model.State;
using Cotillo_ShoppingCart_Services.Integration.Implementation.EF;
using Cotillo_ShoppingCart_Services.Integration.Interfaces.EF;
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

            IRepository<StateProvinceEntity> stateRepository = new EFRepository<StateProvinceEntity>(context);
            //Set the value to compare with
            Expression<Func<StateProvinceEntity, object>> stateFilter = i => i.Name;

            StateProvinceEntity state = new StateProvinceEntity()
            {
                    Active = true,
                    CreatedOn = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    Name = "MA",
                    Country = new CountryEntity()
                    {
                        Active = true,
                        CreatedOn = DateTime.Now,
                        LastUpdated = DateTime.Now,
                        Name = "USA"
                    }
            };
            stateRepository.AddOrUpdate(stateFilter, state);

        }
    }
}
