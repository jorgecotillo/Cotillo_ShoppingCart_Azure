using Cotillo_ShoppingCart_Services.Domain.Model.Country;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Mappings
{
    public class CountryMapping : EntityTypeConfiguration<CountryEntity>
    {
        public CountryMapping()
        {
            this.ToTable("Countries");

            this.HasKey(i => i.Id);

            this.Property(i => i.Name)
                .HasMaxLength(255);

            this.Property(i => i.Active)
                    .IsRequired();

            this.Property(i => i.CreatedOn)
                    .IsRequired();

            this.Property(i => i.LastUpdated)
                    .IsRequired();
        }
    }
}
