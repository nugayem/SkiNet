using System;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o=>o.ShipAddress, a=>
            {
                a.WithOwner();
            });
            builder.Property(s=> s.Status).HasConversion(
                a=>a.ToString(),
                a=> (OrderStatus)Enum.Parse(typeof(OrderStatus),a)
            );
            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}