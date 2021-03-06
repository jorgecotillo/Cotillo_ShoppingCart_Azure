﻿using Cotillo_ShoppingCart_Services.Domain.Model.Order;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Mappings
{
    public class OrderItemMapping : EntityTypeConfiguration<OrderItemEntity>
    {
        public OrderItemMapping()
        {
            this.ToTable("OrderItem");

            this.HasKey(i => i.Id);

            this.HasRequired(i => i.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(i => i.OrderId)
                .WillCascadeOnDelete(false);

            this.HasRequired(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .WillCascadeOnDelete(false);

            this.Property(i => i.PriceExcTax)
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
