using Cotillo_ShoppingCart_Services.Domain.Model.Product;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Mappings
{
    public class CategoryMapping : EntityTypeConfiguration<CategoryEntity>
    {
        public CategoryMapping()
        {
            this.ToTable("Catogories");

            this.HasKey(i => i.Id);

            this.Property(i => i.Name)
                .HasMaxLength(255)
                .IsRequired();

            this.Property(i => i.Active)
                    .IsRequired();

            this.Property(i => i.CreatedOn)
                    .IsRequired();

            this.Property(i => i.LastUpdated)
                    .IsRequired();
        }
    }
}
