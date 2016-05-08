using Cotillo_ShoppingCart_Services.Domain.Model.Addresses;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Mappings
{
    public class AddressMapping : EntityTypeConfiguration<AddressEntity>
    {
        public AddressMapping()
        {
            this.ToTable("Addresses");

            this.HasKey(i => i.Id);

            this.Property(i => i.Address1)
                .HasMaxLength(255);

            this.Property(i => i.Address2)
                .HasMaxLength(255);
            
            this.Property(i => i.Active)
                    .IsRequired();

            this.Property(i => i.CreatedOn)
                    .IsRequired();

            this.Property(i => i.LastUpdated)
                    .IsRequired();

            this.HasRequired(i => i.Country)
                .WithMany()
                .HasForeignKey(i => i.CountryId)
                .WillCascadeOnDelete(false);

            this.HasRequired(i => i.StateProvince)
                .WithMany()
                .HasForeignKey(i => i.StateProvinceId)
                .WillCascadeOnDelete(false);
        }
    }
}
