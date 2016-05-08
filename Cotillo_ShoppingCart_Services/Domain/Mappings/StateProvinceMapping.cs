using Cotillo_ShoppingCart_Services.Domain.Model.State;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Mappings
{
    public class StateProvinceMapping : EntityTypeConfiguration<StateProvinceEntity>
    {
        public StateProvinceMapping()
        {
            this.ToTable("States");

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

            this.HasRequired(i => i.Country)
                .WithMany(ii => ii.StateProvinces)
                .HasForeignKey(i => i.CountryId)
                .WillCascadeOnDelete(false);
        }
    }
}
