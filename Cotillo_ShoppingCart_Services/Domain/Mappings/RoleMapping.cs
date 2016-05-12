using Cotillo_ShoppingCart_Services.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Mappings
{
    public class RoleMapping : EntityTypeConfiguration<RoleEntity>
    {
        public RoleMapping()
        {
            this.ToTable("Roles");

            this.HasKey(i => i.Id);

            this.Property(i => i.Name)
                .HasMaxLength(100)
                .IsOptional();

            this.Property(i => i.Key)
                .HasMaxLength(50)
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
