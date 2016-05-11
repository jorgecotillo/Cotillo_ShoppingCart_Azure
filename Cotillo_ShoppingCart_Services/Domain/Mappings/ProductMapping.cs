using Cotillo_ShoppingCart_Services.Domain.Model.Product;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Mappings
{
    public class ProductMapping : EntityTypeConfiguration<ProductEntity>
    {
        public ProductMapping()
        {
            this.ToTable("Products");

            this.HasKey(i => i.Id);

            this.Property(i => i.Name)
                .HasMaxLength(255)
                .IsRequired();

            this.Property(i => i.Barcode)
                .HasMaxLength(20)
                .IsRequired();

            this.Property(i => i.ExpiresOn)
                .IsRequired();

            this.Property(i => i.FileName)
                .HasMaxLength(100)
                .IsRequired();

            this.Ignore(i => i.Image);

            this.Property(i => i.PriceExcTax)
                .IsOptional();

            this.Property(i => i.PriceIncTax)
                .IsRequired();

            this.Property(i => i.Description)
                .HasMaxLength(255)
                .IsRequired();

            this.Property(i => i.Location)
                .HasMaxLength(155)
                .IsRequired();

            this.Property(i => i.Active)
                    .IsRequired();

            this.Property(i => i.CreatedOn)
                    .IsRequired();

            this.Property(i => i.LastUpdated)
                    .IsRequired();

            this.HasRequired(i => i.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(i => i.CategoryId)
                .WillCascadeOnDelete(false);
        }
    }
}
