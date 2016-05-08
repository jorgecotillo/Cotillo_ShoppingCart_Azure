using Cotillo_ShoppingCart_Services.Domain.Model.Product;
using Cotillo_ShoppingCart_Services.Integration.Implementation.EF;
using Cotillo_ShoppingCart_Services.Integration.Interfaces.EF;
using System;
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
            IRepository<ProductEntity> productRepository = new EFRepository<ProductEntity>(context);

            ProductEntity productEntity = new ProductEntity()
            {
                Active = true,
                CreatedOn = DateTime.Now,
                FileName = "APictureFile1.jpg",
                LastUpdated = DateTime.Now,
                Name = "Milk",
                Barcode = "ABC",
                ExpiresOn = DateTime.Now.AddDays(15),
                Category = new CategoryEntity()
                {
                    Active = true,
                    CreatedOn = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    Name = "Groceries"
                }
            };

            //Set the value to compare with
            Expression<Func<ProductEntity, object>> filter = i => i.Barcode;

            productRepository.AddOrUpdate(filter, productEntity);
        }
    }
}
