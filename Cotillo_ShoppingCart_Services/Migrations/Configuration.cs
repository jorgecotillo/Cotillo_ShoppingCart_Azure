namespace Cotillo_ShoppingCart_Services.Migrations
{
    using Domain.Model.Product;
    using Integration.Implementation.EF;
    using Integration.Interfaces.EF;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public class Configuration : DbMigrationsConfiguration<Cotillo_ShoppingCart_Services.Integration.Implementation.EF.EFContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Cotillo_ShoppingCart_Services.Integration.Implementation.EF.EFContext context)
        {
            //IRepository<ProductEntity> productRepository = new EFRepository<ProductEntity>(context);

            //ProductEntity productEntity = new ProductEntity()
            //{
            //    Active = true,
            //    CreatedOn = DateTime.Now,
            //    FileName = "APictureFile1.jpg",
            //    LastUpdated = DateTime.Now,
            //    Name = "Milk",
            //    Barcode = "ABC",
            //    ExpiresOn = DateTime.Now.AddDays(15),
            //    Category = new CategoryEntity()
            //    {
            //        Active = true,
            //        CreatedOn = DateTime.Now,
            //        LastUpdated = DateTime.Now,
            //        Name = "Groceries"
            //    }
            //};

            //if(!productRepository.Table.Any(i => i.Name == productEntity.Name))
            //{
            //    productRepository.InsertAsync(productEntity, autoCommit: true).Wait();
            //}
        }
    }
}
