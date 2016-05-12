using Cotillo_ShoppingCart_Services.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Mappings
{
    public class UserMapping : EntityTypeConfiguration<UserEntity>
    {
        public UserMapping()
        {
            this.ToTable("Users");

            this.HasKey(i => i.Id);

            this.Property(i => i.DisplayName)
                .HasMaxLength(255)
                .IsRequired();

            this.Property(i => i.UserName)
                .HasMaxLength(150)
                .IsRequired();

            this.Property(i => i.ExternalAccount)
                .HasMaxLength(255)
                .IsOptional();

            this.Property(i => i.Active)
                    .IsRequired();

            this.Property(i => i.CreatedOn)
                    .IsRequired();

            this.Property(i => i.LastUpdated)
                    .IsRequired();

            this.HasMany(i => i.Roles)
                .WithMany(u => u.Users)
                .Map(i =>
                {
                    i.MapLeftKey("UserId");
                    i.MapRightKey("RoleId");
                    i.ToTable("UserRoles");
                });
        }
    }
}
