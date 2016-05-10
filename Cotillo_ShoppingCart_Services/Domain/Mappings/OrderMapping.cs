using Cotillo_ShoppingCart_Services.Domain.Model.Order;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Mappings
{
    public class OrderMapping : EntityTypeConfiguration<OrderEntity>
    {
        public OrderMapping()
        {
            this.ToTable("Orders");

            this.HasKey(i => i.Id);

            this.HasRequired(i => i.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(i => i.CustomerId)
                .WillCascadeOnDelete(false);

            this.Property(i => i.TotalExcTax)
                .IsOptional();

            this.Property(i => i.Active)
                    .IsRequired();

            this.Property(i => i.CreatedOn)
                    .IsRequired();

            this.Property(i => i.LastUpdated)
                    .IsRequired();
        }
    }
}
