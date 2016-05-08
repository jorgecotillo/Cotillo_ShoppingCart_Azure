using Cotillo_ShoppingCart_Services.Domain.Model.Customer;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Mappings
{
    public class CustomerMapping : EntityTypeConfiguration<CustomerEntity>
    {
        public CustomerMapping()
        {
            this.ToTable("Customers");

            this.HasKey(i => i.Id);

            this.Property(i => i.Email)
                .HasMaxLength(255);

            this.Property(i => i.Active)
                    .IsRequired();

            this.Property(i => i.CreatedOn)
                    .IsRequired();

            this.Property(i => i.LastUpdated)
                    .IsRequired();

            this.HasRequired(i => i.BillingAddress)
                .WithMany()
                .HasForeignKey(i => i.BillingAddressId)
                .WillCascadeOnDelete(false);

            this.HasRequired(i => i.User)
                .WithMany()
                .HasForeignKey(i => i.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}
